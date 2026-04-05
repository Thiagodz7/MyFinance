using MediatR;
using MyFinance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyFinance.Domain.Entities.Lancamento;

namespace MyFinance.Application.Commands
{
    public class AlterarLancamentoCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }

        public bool EhRecorrente { get; set; }
        public TipoFrequencia Frequencia { get; set; }
        public int TotalParcelas { get; set; } = 1;
    }
}
