using DiceGameApp.Server.Services;
using DiceGameApp.Shared;

namespace DiceGameApp.Server
{
    public class MatchmakerService : IMatchmakerService
    {
        private readonly IGameService _gameService;

        public MatchmakerService(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<GameSession> FindOrCreateGameAsync(Player player)
        {
            var session = await _gameService.GetNotStartedGameSessionAsync();
            if (session == null)
            {
                session = await _gameService.StartGameSessionAsync(player, null);
            }
            else
            {
                session.Player2 = player;
                session.Player2Id = player.Id;
                session = await _gameService.UpdateGameSessionAsync(session);
            }
            return await Task.FromResult(session);
        }

        public async Task<List<Player>> GetLeaderBoardAsync()
        {
            return await _gameService.GetLeaderBoardAsync(10);
        }
    }
}
