using MassTransit;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure;
using MyFinance.Infrastructure.Data;
using MyFinance.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Adicione estas linhas LOGO NO INÍCIO, após o 'CreateBuilder'
// -----------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
// -----------------------------------------------------------

builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// Registra o MediatR e diz para ele procurar Handlers no projeto "MyFinance.Application"
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CriarLancamentoCommand).Assembly));

builder.Services.AddMassTransit(x =>
{
    // Tenta ler a variável de ambiente RABBITMQ_HOST. Se não tiver, usa localhost.
    var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
    // Configura para usar RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        // A conexão padrão do Docker: localhost na porta 5672
        // Se você mudou a senha no docker-compose, ajuste aqui!
        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Melhora a serialização do JSON
        cfg.ConfigureEndpoints(context);
    });
});

// [CORREÇÃO AQUI] Adiciona o suporte a Controllers
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Gera o JSON em /openapi/v1.json
    app.MapOpenApi();

    // Gera a Interface Visual em /scalar/v1
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers(); // Importante: Garanta que esta linha existe para mapear seu Controller!

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
