using MediatR;

namespace MyFinance.Application.Commands
{
    // O comando retorna um Guid (o ID da conta criada)
    public class CriarContaCommand : IRequest<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty; // Ex: "Nubank", "Itaú"
        public decimal SaldoInicial { get; set; }
    }
}