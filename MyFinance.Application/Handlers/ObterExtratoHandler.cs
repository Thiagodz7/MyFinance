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

            // Traz tudo do banco
            var todosLancamentos = await _lancamentoRepo.GetByContaIdAsync(request.ContaId);

            // =======================================================
            // FILTRO DE COMPETÊNCIA: Exibe apenas o que importa no momento (Mês Atual)
            // =======================================================
            var dataAtual = DateTime.Now;
            var lancamentosDoMes = todosLancamentos
                .Where(l => l.DataVencimento.Month == dataAtual.Month && l.DataVencimento.Year == dataAtual.Year)
                .ToList();

            // O Saldo no Extrato agora reflete o Resultado Líquido do Mês (Para bater com o Excel)
            var balancoDoMes = lancamentosDoMes.Sum(l => l.Valor);

            return new ExtratoDto
            {
                ContaId = conta.Id,
                NomeConta = conta.Nome,
                Banco = conta.Banco,
                SaldoAtual = balancoDoMes, // Trocamos o Saldo Global pelo Saldo do Mês Visível
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