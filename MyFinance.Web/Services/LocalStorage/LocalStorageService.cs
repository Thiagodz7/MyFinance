using Microsoft.JSInterop;

namespace MyFinance.Web.Services.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetItemAsync(string key, string value)
        {
            // Chama o comando nativo do JS: localStorage.setItem('chave', 'valor')
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public async Task<string> GetItemAsync(string key)
        {
            // Chama o comando nativo do JS: localStorage.getItem('chave')
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        public async Task RemoveItemAsync(string key)
        {
            // Chama o comando nativo do JS: localStorage.removeItem('chave')
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}