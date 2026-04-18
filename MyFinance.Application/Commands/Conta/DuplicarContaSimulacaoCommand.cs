using MediatR;

namespace MyFinance.Application.Commands.Conta
{
    public class DuplicarContaSimulacaoCommand : IRequest<Guid>
    {
        public Guid ContaOriginalId { get; set; }
        public string NomeSimulacao { get; set; } = string.Empty;
    }
}
