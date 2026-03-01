using Microsoft.AspNetCore.Components.Authorization;
using MyFinance.Web.Auth;
using MyFinance.Web.DTOs.Auth;
using System.Net.Http.Json;

namespace MyFinance.Web.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> Login(LoginUserDto loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/entrar", loginModel);

            if (result.IsSuccessStatusCode)
            {
                // A API retorna o token como uma string simples
                var token = await result.Content.ReadAsStringAsync();

                // AQUI A MÁGICA ACONTECE:
                // Avisamos o Provider que o usuário logou
                await ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                        .MarkUserAsAuthenticated(token);

                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                    .MarkUserAsLoggedOut();
        }

        public async Task<bool> Register(RegisterUserDto registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/nova-conta", registerModel);

            if (result.IsSuccessStatusCode)
            {
                // Se o registro já logar direto, pegamos o token aqui também
                var token = await result.Content.ReadAsStringAsync();
                await ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                        .MarkUserAsAuthenticated(token);
                return true;
            }
            return false;
        }
    }
}
