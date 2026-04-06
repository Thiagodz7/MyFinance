using MediatR;
using MyFinance.Application.DTOs;

namespace MyFinance.Application.Queries
{
    // IRequest<ExtratoDto>: Esse pedido devolve um ExtratoDto
    public class ObterExtratoQuery : IRequest<ExtratoDto>
    {
        public Guid ContaId { get; set; }
        public int? Mes { get; set; }
        public int? Ano { get; set; }

        public ObterExtratoQuery(Guid contaId, int? mes = null, int? ano = null)
        {
            ContaId = contaId;
            Mes = mes;
            Ano = ano;
        }
    }
}