using IndexedDB.Blazor;
using IndexedDB.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using MyFinance.Web;
using MyFinance.Web.Auth;
using MyFinance.Web.Handlers;
using MyFinance.Web.Services;
using MyFinance.Web.Services.Auth;
using MyFinance.Web.Services.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddTransient<UnauthorizedInterceptor>();

// Configura o HttpClient para apontar para o Docker (localhost:8080)
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://187.77.228.172:8080/") });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://myfinancesxls.tech/") });

//if (builder.HostEnvironment.IsDevelopment())
//{
//    // Em Dev, o Blazor vai chamar a API que está rodando no seu Docker local na porta 8080
//    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8080/") });
//}
//else
//{
//    // Em Prod, chama o domínio real
//    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://myfinancesxls.tech/") });
//}

builder.Services.AddScoped(sp =>
{
    var interceptor = sp.GetRequiredService<UnauthorizedInterceptor>();

    // O InnerHandler é o motor padrão do Blazor para fazer requisições
    interceptor.InnerHandler = new HttpClientHandler();

    // Aquela nossa lógica de Dev vs Prod continua aqui:
    var baseAddress = builder.HostEnvironment.IsDevelopment()
        ? "http://localhost:8080/"
        : "https://myfinancesxls.tech/";

    return new HttpClient(interceptor)
    {
        BaseAddress = new Uri(baseAddress)
    };
});

builder.Services.AddSingleton<IIndexedDbFactory, IndexedDbFactory>();

builder.Services.AddScoped<MyFinanceLocalDb>(sp => {
    var jsRuntime = sp.GetRequiredService<IJSRuntime>();
    return new MyFinanceLocalDb(jsRuntime);
});

builder.Services.AddScoped<PageStateService>();
builder.Services.AddScoped<FinanceStateService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

// 2. Registre nosso provedor customizado
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 1. Adicione suporte a Autorização (Essencial)
builder.Services.AddAuthorizationCore();


// Testar No Celular
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://192.168.1.9:8080") });


// Adiciona os serviços do MudBlazor
//builder.Services.AddMudServices();

// Substitua o "builder.Services.AddMudServices();" por este bloco:
builder.Services.AddMudServices(config =>
{
    // Configuração Global do Snackbar
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

await builder.Build().RunAsync();
