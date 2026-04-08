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

            // VERIFICAÇÃO DE VISÃO GLOBAL VS INDIVIDUAL
            if (request.ContaId == Guid.Empty)
            {
                // 1. Busca todas as contas do usuário para saber quais lançamentos pertencem a ele
                var contas = await _contaRepo.GetAllAsync();
                var idsContas = contas.Select(c => c.Id).ToList();

                // 2. Busca os lançamentos e filtra pelos IDs das contas encontradas
                var lancamentosBrutos = await _lancamentoRepo.GetAllAsync();
                todosLancamentos = lancamentosBrutos.Where(l => idsContas.Contains(l.ContaId)).ToList();

                nomeExibicao = "Todas as Contas";
                bancoExibicao = "Consolidado Pejota.io";
            }
            else
            {
                var conta = await _contaRepo.GetByIdAsync(request.ContaId);
                if (conta == null) throw new Exception("Conta não encontrada");

                // Adicione o parênteses e o .ToList() aqui também!
                todosLancamentos = (await _lancamentoRepo.GetByContaIdAsync(request.ContaId)).ToList();

                nomeExibicao = conta.Nome;
                bancoExibicao = conta.Banco;
            }

            // LÓGICA DE FILTRO TEMPORAL (Mês/Ano)
            var mesAlvo = request.Mes ?? DateTime.Now.Month;
            var anoAlvo = request.Ano ?? DateTime.Now.Year;
            var dataFiltroInicio = new DateTime(anoAlvo, mesAlvo, 1);

            // CÁLCULO DO TRIPÉ FINANCEIRO
            // 1. Saldo Anterior (Tudo o que aconteceu antes do mês selecionado)
            var saldoPassado = todosLancamentos
                .Where(l => l.DataVencimento < dataFiltroInicio)
                .Sum(l => l.Valor);

            // 2. Lançamentos do Mês (O que vai para a Grid)
            var lancamentosDoMes = todosLancamentos
                .Where(l => l.DataVencimento.Month == mesAlvo && l.DataVencimento.Year == anoAlvo)
                .ToList();

            // 3. Lucro Líquido do Mês
            var lucroMesAtual = lancamentosDoMes.Sum(l => l.Valor);

            return new ExtratoDto
            {
                ContaId = request.ContaId,
                NomeConta = nomeExibicao,
                Banco = bancoExibicao,

                // Povoando o Tripé
                SaldoAnterior = saldoPassado,
                LucroDoMes = lucroMesAtual,
                SaldoAtual = saldoPassado + lucroMesAtual,

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