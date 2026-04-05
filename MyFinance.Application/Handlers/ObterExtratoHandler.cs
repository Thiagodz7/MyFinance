using MediatR;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class ObterExtratoHandler : IRequestHandler<ObterExtratoQuery, ExtratoDto>
    {
        private readonly IContaRepository _contaRepo;
        private readonly ILancamentoRepository _lancamentoRepo;

        public ObterExtratoHandler(IContaRepository contaRepo, ILancamentoRepository lancamentoRepo)
        {
            _contaRepo = contaRepo;
            _lancamentoRepo = lancamentoRepo;
        }

        public async Task<ExtratoDto> Handle(ObterExtratoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepo.GetByIdAsync(request.ContaId);

            if (conta == null)
                throw new Exception("Conta não encontrada");

            // Trazemos tudo para poder calcular a história da conta
            var todosLancamentos = await _lancamentoRepo.GetByContaIdAsync(request.ContaId);

            var dataAtual = DateTime.Now;

            // 1. O que aconteceu antes deste mês (Para o Saldo Anterior)
            // Pegamos tudo até o último dia do mês passado
            var saldoPassado = todosLancamentos
                .Where(l => l.DataVencimento < new DateTime(dataAtual.Year, dataAtual.Month, 1))
                .Sum(l => l.Valor);

            // 2. O que aconteceu DENTRO deste mês (Para o Extrato Visual)
            var lancamentosDoMes = todosLancamentos
                .Where(l => l.DataVencimento.Month == dataAtual.Month && l.DataVencimento.Year == dataAtual.Year)
                .ToList();

            // 3. Resultado puramente do mês (Receitas - Despesas de Abril)
            var lucroMesAtual = lancamentosDoMes.Sum(l => l.Valor);

            return new ExtratoDto
            {
                ContaId = conta.Id,
                NomeConta = conta.Nome,
                Banco = conta.Banco,

                // --- Povoando o Tripé ---
                SaldoAnterior = saldoPassado,
                LucroDoMes = lucroMesAtual,
                SaldoAtual = saldoPassado + lucroMesAtual, // A soma perfeita

                Lancamentos = lancamentosDoMes.Select(l => new LancamentoDto
                {
                    Id = l.Id,
                    Descricao = l.Descricao,
                    Valor = l.Valor,
                    Categoria = l.Categoria?.Nome ?? "Sem Categoria",
                    Data = l.DataVencimento,
                    Tipo = l.Valor >= 0 ? "Receita" : "Despesa",
                    CategoriaId = l.CategoriaId,
                    ContaId = l.ContaId,
                    Pago = l.Pago,
                    EhRecorrente = l.EhRecorrente,
                    Frequencia = (int)l.Frequencia,
                    ParcelaAtual = l.ParcelaAtual,
                    TotalParcelas = l.TotalParcelas,
                    GrupoRecorrenciaId = l.GrupoRecorrenciaId
                }).ToList()
            };
        }
    }
}