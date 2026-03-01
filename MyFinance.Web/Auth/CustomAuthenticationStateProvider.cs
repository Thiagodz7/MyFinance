using Microsoft.AspNetCore.Components.Authorization;
using MyFinance.Web.Services; // Importe seu ILocalStorageService aqui
using MyFinance.Web.Services.LocalStorage;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace MyFinance.Web.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        // 1. O Método Principal: O Blazor chama isso toda vez que precisa saber quem é o usuário
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Tenta pegar o token salvo no navegador
            var token = await _localStorage.GetItemAsync("authToken");

            // Se não tiver token, retorna usuário "Anônimo" (Deslogado)
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // SE tiver token:
            // a) Avisa o HttpClient para incluir esse token em TODAS as próximas requisições
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // b) Lê as claims (dados) do token e cria o Usuário
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        // 2. Método para Logar (Chamaremos na tela de Login)
        public async Task MarkUserAsAuthenticated(string token)
        {
            await _localStorage.SetItemAsync("authToken", token);

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            // Configura o cabeçalho HTTP imediatamente
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Avisa todos os componentes que o estado mudou!
            NotifyAuthenticationStateChanged(authState);
        }

        // 3. Método para Deslogar (Chamaremos no botão Sair)
        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync("authToken");

            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));

            // Remove o cabeçalho HTTP
            _http.DefaultRequestHeaders.Authorization = null;

            NotifyAuthenticationStateChanged(authState);
        }

        // --- MÁGICA PARA LER O JWT SEM BIBLIOTECAS PESADAS ---
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1]; // Pega o miolo do token
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null) return claims;

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}