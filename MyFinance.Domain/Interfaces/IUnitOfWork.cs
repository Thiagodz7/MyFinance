namespace MyFinance.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
