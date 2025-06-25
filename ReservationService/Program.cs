using Microsoft.EntityFrameworkCore;
using ReservationCache;
using ReservationService;
using ReservationService.Model;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja po³¹czenia Redis jako singleton

// Rejestracja w³asnej us³ugi cache, która zale¿y od IConnectionMultiplexer
builder.Services.AddScoped<IReservationCache, ReservationClass>();
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
// Add services to the container.
builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var redisHost = builder.Configuration["Redis:Host"] ?? "localhost";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisHost));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ResponseTimeMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
