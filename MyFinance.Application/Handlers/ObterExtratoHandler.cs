using MediatR;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;
using MyFinance.Domain.Entities;
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
            List<Lancamento> todosLancamentos;
            string nomeExibicao;
            string bancoExibicao;

            if (request.ContaId == Guid.Empty)
            {
                var contas = await _contaRepo.GetAllAsync();
                var idsContas = contas.Select(c => c.Id).ToList();

                var lancamentosBrutos = await _lancamentoRepo.GetAllAsync();
                todosLancamentos = lancamentosBrutos.Where(l => idsContas.Contains(l.ContaId)).ToList();

                nomeExibicao = "Todas as Contas";
                bancoExibicao = "Consolidado Pejota.io";
            }
            else
            {
                var conta = await _contaRepo.GetByIdAsync(request.ContaId);
                if (conta == null) throw new Exception("Conta não encontrada");

                todosLancamentos = (await _lancamentoRepo.GetByContaIdAsync(request.ContaId)).ToList();

                nomeExibicao = conta.Nome;
                bancoExibicao = conta.Banco;
            }

            var mesAlvo = request.Mes ?? DateTime.Now.Month;
            var anoAlvo = request.Ano ?? DateTime.Now.Year;
            var dataFiltroInicio = new DateTime(anoAlvo, mesAlvo, 1);

            var saldoPassado = todosLancamentos
                .Where(l => l.DataVencimento < dataFiltroInicio)
                .Sum(l => l.Valor);

            var lancamentosDoMes = todosLancamentos
                .Where(l => l.DataVencimento.Month == mesAlvo && l.DataVencimento.Year == anoAlvo)
                .ToList();

            var lucroMesAtual = lancamentosDoMes.Sum(l => l.Valor);

            var dataFiltroFim = new DateTime(anoAlvo, mesAlvo, DateTime.DaysInMonth(anoAlvo, mesAlvo), 23, 59, 59);
            var saldoReal = todosLancamentos.Where(l => l.Pago && l.DataVencimento <= dataFiltroFim)
                                            .Sum(l => l.Valor);

            return new ExtratoDto
            {
                ContaId = request.ContaId,
                NomeConta = nomeExibicao,
                Banco = bancoExibicao,

                SaldoAnterior = saldoPassado,
                LucroDoMes = lucroMesAtual,
                SaldoAtual = saldoPassado + lucroMesAtual,
                SaldoReal = saldoReal,

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
                }).OrderBy(l => l.Data).ToList()
            };
        }
    }
}