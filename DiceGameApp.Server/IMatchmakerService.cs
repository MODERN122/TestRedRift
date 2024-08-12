using DiceGameApp.Shared;

namespace DiceGameApp.Server
{
    public interface IMatchmakerService
    {
        Task<GameSession> FindOrCreateGameAsync(Player player);
        Task<List<Player>> GetLeaderBoardAsync();
    }
}
