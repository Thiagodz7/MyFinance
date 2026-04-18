using MediatR;
using MyFinance.Shared.Enums;

namespace MyFinance.Application.Commands
{
    // O comando retorna um Guid (o ID da conta criada)
    public class CriarContaCommand : IRequest<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty; 
        public decimal SaldoInicial { get; set; }
        public TipoConta Tipo { get; set; } = TipoConta.Corrente;
    }
}