using MediatR;
using MyFinance.Application.DTOs;

namespace MyFinance.Application.Queries
{
    public class ObterDashboardQuery : IRequest<DashboardDto>
    {
        public Guid ContaId { get; set; }

        public ObterDashboardQuery(Guid contaId)
        {
            ContaId = contaId;
        }
    }
}