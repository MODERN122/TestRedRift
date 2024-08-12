using DiceGameApp.Shared;

namespace DiceGameApp.Server.Services
{
    public interface IGameService
    {
        Task<Player> CreatePlayerAsync(Player player);
        Task<GameSession> StartGameSessionAsync(Player player1, Player player2);
        Task<Turn> ProcessPlayerTurnAsync(string sessionId, string playerId, bool useSpecialDice);
        Task<GameSession> GetGameSessionAsync(string sessionId);
        Task<GameSession> UpdateGameSessionAsync(GameSession gameSession);
        Task<GameSession> GetNotStartedGameSessionAsync();
        Task EndGameSessionAsync(string sessionId, string? loserId = null);
        Task<GameSession> GetCurrentGameStateAsync(string sessionId);
        Task<List<Player>> GetLeaderBoardAsync(int limit = 10);
    }
}
