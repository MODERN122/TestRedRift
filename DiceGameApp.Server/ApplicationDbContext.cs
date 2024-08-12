using Microsoft.EntityFrameworkCore;
using DiceGameApp.Shared;

namespace DiceGameApp.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Дополнительные настройки для сущностей
            modelBuilder.Entity<GameSession>()
                .HasKey(gs => gs.Id);

            modelBuilder.Entity<Turn>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Player>()
                .HasKey(p => p.Id);

            // Связь между GameSession и Player
            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.Player1)
                .WithMany()
                .HasForeignKey(gs => gs.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.Player2)
                .WithMany()
                .HasForeignKey(gs => gs.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь между Turn и GameSession
            modelBuilder.Entity<Turn>()
                .HasOne(t => t.GameSession)
                .WithMany(gs => gs.Turns)
                .HasForeignKey(t => t.GameSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
