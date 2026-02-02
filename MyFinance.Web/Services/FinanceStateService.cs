namespace MyFinance.Web.Services
{
    public class FinanceStateService
    {
        public Guid? SelectedAccountId { get; private set; }
        public string SelectedAccountName { get; private set; } = "Todas as Contas";

        // Evento para notificar as páginas (Dashboard, Extrato) que a conta mudou
        public event Action? OnAccountChanged;

        public void SetAccount(Guid? id, string name)
        {
            if (SelectedAccountId != id)
            {
                SelectedAccountId = id;
                SelectedAccountName = name;
                OnAccountChanged?.Invoke();
            }
        }
    }
}
