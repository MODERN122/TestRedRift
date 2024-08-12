namespace DiceGameApp.Shared
{
    public class GameSession
    {
        public string Id { get; set; }
        public Player Player1 { get; set; }
        public Player? Player2 { get; set; }
        public string Player1Id { get; set; }
        public string? Player2Id { get; set; }
        public int TurnNumber { get; set; }
        public bool IsFinished { get; set; } = false;
        public List<Turn> Turns { get; set; } = new List<Turn>();
        public string? WinnerId { get; set; }
    }
}
