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

            // 1. Define qual é o mês/ano alvo da busca
            var mesAlvo = request.Mes ?? DateTime.Now.Month;
            var anoAlvo = request.Ano ?? DateTime.Now.Year;
            var dataFiltroInicio = new DateTime(anoAlvo, mesAlvo, 1);

            // 2. O que aconteceu antes do mês filtrado (Para o Saldo Anterior dinâmico)
            var saldoPassado = todosLancamentos
                .Where(l => l.DataVencimento < dataFiltroInicio)
                .Sum(l => l.Valor);

            // 3. O que aconteceu DENTRO do mês filtrado
            var lancamentosDoMes = todosLancamentos
                .Where(l => l.DataVencimento.Month == mesAlvo && l.DataVencimento.Year == anoAlvo)
                .ToList();

            // 4. Resultado puramente do mês selecionado
            var lucroMesAtual = lancamentosDoMes.Sum(l => l.Valor);

            return new ExtratoDto
            {
                ContaId = conta.Id,
                NomeConta = conta.Nome,
                Banco = conta.Banco,

                // --- Povoando o Tripé Dinâmico ---
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
                }).OrderBy(l => l.Data).ToList() // Ordena por data para o UI ficar alinhado
            };
        }
    }
}