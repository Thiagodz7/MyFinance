using MassTransit;
using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure;
using MyFinance.Infrastructure.Data;
using MyFinance.Infrastructure.Repositories;
using MyFinance.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

// [MUDANÇA AQUI] Ler a ConnectionString da configuração (que virá do Docker)
// Se não encontrar (rodando local), usa uma string padrão ou null.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Injeção de Dependência do Repositório de Contas
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configuração do MassTransit
builder.Services.AddMassTransit(x =>
{
    // 1. Avisa que existe esse Consumidor
    x.AddConsumer<LancamentoCriadoConsumer>();

    // 2. Configura a conexão com RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        // Tenta ler a variável de ambiente RABBITMQ_HOST. Se não tiver, usa localhost.
        var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";

        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // 3. O Pulo do Gato: ReceiveEndpoint
        // Isso cria a FILA "fila-lancamentos" lá no RabbitMQ automaticamente
        cfg.ReceiveEndpoint("fila-lancamentos", e =>
        {
            e.ConfigureConsumer<LancamentoCriadoConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();