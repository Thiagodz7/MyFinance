using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyFinance.API.Services;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;
using MyFinance.Identity;
using MyFinance.Identity.SecurityContext;
using MyFinance.Infrastructure;
using MyFinance.Infrastructure.Data;
using MyFinance.Infrastructure.Repositories;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adicione estas linhas LOGO NO INÍCIO, após o 'CreateBuilder'
// -----------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Habilita tentativas automáticas em caso de erro transiente (como banco iniciando)
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>() 
    .AddEntityFrameworkStores<SecurityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<SecurityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), // Usa o mesmo banco
        b => b.MigrationsAssembly("MyFinance.Identity") // <--- O PULO DO GATO!
    ));

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var chave = Encoding.ASCII.GetBytes(jwtSettings["Segredo"]!);

builder.Services.AddAuthentication(options =>
{
    // Aqui dizemos: "O padrão é JWT, não use Cookie!"
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Em dev pode ser false
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chave),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Emissor"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Publico"]
    };
});

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
        // Lê do appsettings (local) ou das variáveis de ambiente do Docker (produção)
        var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
        var rabbitPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";

        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        // Melhora a serialização do JSON
        cfg.ConfigureEndpoints(context);
    });
});

// [CORREÇÃO AQUI] Adiciona o suporte a Controllers
builder.Services.AddControllers();


builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi(); versão antiga

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // 1. Define o esquema de segurança (O "Cadeado")
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insira o token JWT gerado no endpoint de login."
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", securityScheme);

        // 2. Aplica a segurança globalmente (A "Chave")
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        return Task.CompletedTask;
    });
});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => {
        builder.WithOrigins(
                "https://myfinancesxls.tech",
                "https://www.myfinancesxls.tech" // Adicionando o WWW aqui!
               )
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

//Quando quiser limitar o acesso
//builder.Services.AddCors(options => {
//    options.AddPolicy("ProductionPolicy", builder => {
//        builder.WithOrigins("https://myfinancesxls.tech") 
//               .AllowAnyHeader()
//               .AllowAnyMethod();
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Gera o JSON em /openapi/v1.json
    app.MapOpenApi();

    // Gera a Interface Visual em /scalar/v1
    app.MapScalarApiReference();
}

//Cria o banco e as tabelas se não existirem
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // 1. Migra as tabelas de Finanças (Contas, Lançamentos, etc)
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        // 2. Migra as tabelas de Segurança (Usuários, Logins, Roles)
        var securityContext = services.GetRequiredService<SecurityDbContext>();
        securityContext.Database.Migrate();

        Console.WriteLine("--> Migrations aplicadas com sucesso!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations no banco de dados.");
    }
}

app.UseHttpsRedirection();

// 2. Ative o Middleware de CORS (Logo no começo do pipeline)
app.UseCors("AllowAll");

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers(); // Importante: Garanta que esta linha existe para mapear seu Controller!


app.Run();

