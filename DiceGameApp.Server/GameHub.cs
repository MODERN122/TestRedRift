using DiceGameApp.Server.Services;
using DiceGameApp.Shared;
using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    private readonly IGameService _gameService;

    public GameHub(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task SendRollResult(GameSession session)
    {
        await Clients.Group(session.Id).SendAsync("ReceiveRollResult", session);
    }

    public async Task RollDice(string sessionId, string playerId, bool useSpecialDice)
    {
        var turn = await _gameService.ProcessPlayerTurnAsync(sessionId, playerId, useSpecialDice);
        var session = await _gameService.GetGameSessionAsync(sessionId);
        await SendRollResult(session);
    }

    public async Task JoinGame(string sessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        var session = await _gameService.GetGameSessionAsync(sessionId);
        await SendRollResult(session);
    }
}
