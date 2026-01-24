using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyFinance.Web;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Configura o HttpClient para apontar para o Docker (localhost:8080)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8080/") });

// Testar No Celular
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://192.168.1.11:8080") });

// Adiciona os serviços do MudBlazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();
