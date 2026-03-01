namespace MyFinance.Web.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        // Salvar um item (ex: o Token)
        Task SetItemAsync(string key, string value);

        // Ler um item
        Task<string> GetItemAsync(string key);

        // Remover (Logout)
        Task RemoveItemAsync(string key);
    }
}
