using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using MyFinance.Web;
using MyFinance.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Configura o HttpClient para apontar para o Docker (localhost:8080)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8080/") });

builder.Services.AddScoped<PageStateService>();
builder.Services.AddScoped<FinanceStateService>();


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
