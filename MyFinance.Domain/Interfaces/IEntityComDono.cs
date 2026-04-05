namespace MyFinance.Domain.Interfaces
{
    public interface IEntityComDono
    {
        string UserId { get; }
        void AssociarUsuario(string userId);
    }
}
