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
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// Configuração do MassTransit
builder.Services.AddMassTransit(x =>
{
    // 1. Avisa que existe esse Consumidor
    x.AddConsumer<LancamentoCriadoConsumer>();

    // 2. Configura a conexão com RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        // Lê do appsettings (local) ou das variáveis de ambiente do Docker (produção)
        var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "admin";
        var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "SuaSenhaForteRabbit!";

        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        // 3. O Pulo do Gato: ReceiveEndpoint
        cfg.ReceiveEndpoint("fila-lancamentos", e =>
        {
            e.ConfigureConsumer<LancamentoCriadoConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();