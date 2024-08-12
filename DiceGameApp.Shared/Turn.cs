using System;

namespace DiceGameApp.Shared
{
    public class Turn
    {
        public int Id { get; set; }  // Уникальный идентификатор хода

        public string GameSessionId { get; set; }  // Идентификатор игровой сессии
        public GameSession GameSession { get; set; }

        public string PlayerId { get; set; }  // Идентификатор игрока, совершившего ход

        public int? DiceRoll1 { get; set; }  // Результат первого броска (если используется стандартный набор костей)
        public int? DiceRoll2 { get; set; }  // Результат второго броска (если используется стандартный набор костей)
        public int? DiceRoll3 { get; set; }  // Результат третьего броска (если используется стандартный набор костей)
        public int? TurnNumber { get; set; } // Номер хода игрока. Нечётный - Player1, чётный - Player2 

        public int? SpecialDiceRoll { get; set; }  // Результат броска специального кубика

        public DateTime TurnTime { get; set; }  // Время совершения хода

        public bool IsCompleted { get; set; }  // Флаг, указывающий, завершен ли ход
    }
}