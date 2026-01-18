using MediatR;
using MyFinance.Application.DTOs;

namespace MyFinance.Application.Queries
{
    // IRequest<ExtratoDto>: Esse pedido devolve um ExtratoDto
    public class ObterExtratoQuery : IRequest<ExtratoDto>
    {
        public Guid ContaId { get; set; }

        public ObterExtratoQuery(Guid contaId)
        {
            ContaId = contaId;
        }
    }
}