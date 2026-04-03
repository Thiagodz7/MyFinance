using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MyFinance.Web.Handlers; // Ajuste para o seu namespace

public class UnauthorizedInterceptor : DelegatingHandler
{
    private readonly NavigationManager _navigationManager;
    private readonly IJSRuntime _jsRuntime;

    public UnauthorizedInterceptor(NavigationManager navigationManager, IJSRuntime jsRuntime)
    {
        _navigationManager = navigationManager;
        _jsRuntime = jsRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Deixa a requisição tentar ir para a API normalmente
        var response = await base.SendAsync(request, cancellationToken);

        // Se a API barrar por token expirado ou falta de permissão...
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // 1. Limpa o token morto do navegador 
            // ATENÇÃO: Troque "authToken" pelo nome exato da chave que você usa no seu LocalStorage!
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, "authToken");

            // 2. Manda o usuário para a tela de login
            _navigationManager.NavigateTo("/login");
        }

        return response;
    }
}