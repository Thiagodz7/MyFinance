// MyFinance.Web/Services/PageStateService.cs
using MyFinance.Web.DTOs;
using static System.Net.WebRequestMethods;

namespace MyFinance.Web.Services
{
    public class PageStateService
    {
        // O título atual da página
        public string Title { get; private set; } = "Dashboard";

        // Evento que avisa os interessados que o título mudou
        public event Action? OnTitleChanged;
        public event Action? OnCreateNewContaChanged;

        public void SetTitle(string newTitle)
        {
            if (Title != newTitle)
            {
                Title = newTitle;
                // Dispara o evento de notificação
                OnTitleChanged?.Invoke();
            }
        }
        public void NotifyCreateNewContaChanged()
        {          
            OnCreateNewContaChanged?.Invoke();
        }
    }
}