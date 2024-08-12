using Azure.Core;
using DiceGameApp.Server;
using DiceGameApp.Server.Data;
using DiceGameApp.Server.Services;
using DiceGameApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Регистрация DbContext с использованием строки подключения из конфигурации
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IMatchmakerService, MatchmakerService>();
builder.Services.AddCors(o => o.AddPolicy("Test", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Настройка и регистрация сервисов Blazor Server
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
// Регистрация SignalR хаба
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("Test");
app.UseRouting();
app.MapHub<GameHub>("/gameHub");
app.MapGet("api/matchmaker/{name}", ([FromRoute] string name, [FromServices] IMatchmakerService ms) => ms.FindOrCreateGameAsync(new Player
{
    Id = Guid.NewGuid().ToString(),
    Name = name,
    Score = 0,
}));
app.MapGet("api/leaderboard", ([FromServices] IMatchmakerService ms) => ms.GetLeaderBoardAsync());

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();