using System;
using System.Linq;
using System.Threading.Tasks;
using DiceGameApp.Server.Data;
using DiceGameApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace DiceGameApp.Server.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _dbContext;

        public GameService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();

            return player;
        }

        public async Task<GameSession> UpdateGameSessionAsync(GameSession gameSession)
        {
            _dbContext.GameSessions.Update(gameSession);
            await _dbContext.SaveChangesAsync();

            return gameSession;
        }

        public async Task<GameSession> StartGameSessionAsync(Player player1, Player player2)
        {
            var session = new GameSession
            {
                Id = Guid.NewGuid().ToString(),
                Player1 = player1,
                Player1Id = player1.Id,
                TurnNumber = 1,
                IsFinished = false
            };

            _dbContext.GameSessions.Add(session);
            await _dbContext.SaveChangesAsync();

            return session;
        }

        public async Task<List<Player>> GetLeaderBoardAsync(int limit = 10)
        {
            var players = await _dbContext.Players.OrderByDescending(_ => _.Score).Take(limit).ToListAsync();
            return players;
        }


        public async Task<Turn> HandleRollAsync(GameSession session, string playerId, bool useSpecialDice)
        {
            var turn = new Turn
            {
                GameSessionId = session.Id,
                PlayerId = playerId,
                TurnTime = DateTime.UtcNow,
                TurnNumber = session.TurnNumber,
                IsCompleted = true
            };

            if (useSpecialDice)
            {
                turn.SpecialDiceRoll = RollSpecialDice();
            }
            else
            {
                turn.DiceRoll1 = RollNormalDice();
                turn.DiceRoll2 = RollNormalDice();
                turn.DiceRoll3 = RollNormalDice();
            }

            _dbContext.Turns.Add(turn);
            await _dbContext.SaveChangesAsync();

            return turn;
        }

        public async Task<Turn> ProcessPlayerTurnAsync(string sessionId, string playerId, bool useSpecialDice)
        {
            var session = await _dbContext.GameSessions
                .Include(s => s.Player1)
                .Include(s => s.Player2)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
            {
                throw new InvalidOperationException("Session was not found!");
            }
            else if (session.IsFinished)
            {
                throw new InvalidOperationException("Session was finished!");
            }

            var turnPlayerId = session.TurnNumber % 2 == 1 ? session.Player1Id : session.Player2Id;

            if (turnPlayerId != playerId)
            {
                throw new InvalidOperationException($"Turn of player {turnPlayerId}");
            }

            var turn = await HandleRollAsync(session, playerId, useSpecialDice);

            session.TurnNumber++;
            _dbContext.GameSessions.Update(session);
            await _dbContext.SaveChangesAsync();

            // Проверка завершения игры
            if (turn.SpecialDiceRoll == 0)
            {
                await EndGameSessionAsync(session.Id, playerId);
            }
            else if (session.TurnNumber > 6)
            {
                await EndGameSessionAsync(session.Id);
            }

            return turn;
        }

        public async Task<GameSession> GetGameSessionAsync(string sessionId)
        {
            var session = await _dbContext.GameSessions
                .AsNoTracking()
                .Include(s => s.Player1)
                .Include(s => s.Player2)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session != null)
            {
                var turns = await _dbContext.Turns
                .AsNoTracking().Where(t => t.GameSessionId == sessionId).ToListAsync();
                session.Turns = turns;
            }

            return session;
        }

        public async Task<GameSession> GetNotStartedGameSessionAsync()
        {
            return await _dbContext.GameSessions
                .Include(s => s.Player1)
                .Include(s => s.Player2)
                .FirstOrDefaultAsync(s => s.Player2 == null);
        }

        public async Task EndGameSessionAsync(string sessionId, string? loserId = null)
        {
            var session = await _dbContext.GameSessions.FindAsync(sessionId);
            if (session != null)
            {
                session.IsFinished = true;

                // Определение победителя и обновление статистики игроков
                var player1Score = CalculatePlayerScore(session, session.Player1.Id);
                var player2Score = CalculatePlayerScore(session, session.Player2.Id);

                session.Player1.Score = player1Score;
                session.Player2.Score = player2Score;

                if (loserId != null)
                {
                    session.WinnerId = session.Player1.Id == loserId ? session.Player2.Id : session.Player1.Id;
                }
                else
                {
                    session.WinnerId = player1Score > player2Score ? session.Player1.Id : session.Player2.Id;
                }

                _dbContext.GameSessions.Update(session);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<GameSession> GetCurrentGameStateAsync(string sessionId)
        {
            return await GetGameSessionAsync(sessionId);
        }

        private int RollNormalDice()
        {
            var random = new Random();
            return random.Next(1, 7);
        }

        private int RollSpecialDice()
        {
            var random = new Random();
            int roll = random.Next(1, 7);
            return roll == 1 ? 0 : roll == 6 ? 24 : roll;
        }

        private int CalculatePlayerScore(GameSession session, string playerId)
        {
            return _dbContext.Turns
                .Where(t => t.GameSessionId == session.Id && t.PlayerId == playerId)
                .Sum(t => (t.SpecialDiceRoll ?? 0) + (t.DiceRoll1 ?? 0) + (t.DiceRoll2 ?? 0) + (t.DiceRoll3 ?? 0));
        }
    }
}
