using ddd.Application;

using ddd.Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// ✅ MediatR v12+ — nowy sposób rejestracji
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommandHandler>();
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// ✅ FluentValidation — rejestracja walidatorów
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();


builder.Services.AddDbContext<AppDbContext>(options =>
options.UseInMemoryDatabase("OrdersDb"));

builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();