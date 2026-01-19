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
        public ObterExtratoHandler(IContaRepository contaRepo, ILancamentoRepository lancamentoRepo )
        {
            _contaRepo = contaRepo;
            _lancamentoRepo = lancamentoRepo;
        }

        public async Task<ExtratoDto> Handle(ObterExtratoQuery request, CancellationToken cancellationToken)
        {
            // 1.0 Busca os dados da Conta (pra saber o saldo)
            var conta = await _contaRepo.GetByIdAsync(request.ContaId);

            if (conta == null)
                throw new Exception("Conta não encontrada"); // Em prod, usaria uma exceção customizada ou Notification Pattern

            // 2. Busca os lançamentos dessa conta
            var lancamentos = await _lancamentoRepo.GetByContaIdAsync(request.ContaId);

            // 3. Monta o objeto de resposta (Mapeamento manual)
            // Em projetos grandes usamos AutoMapper, mas manual é mais rápido e explícito pra aprender
            return new ExtratoDto
            {
                ContaId = conta.Id,
                NomeConta = conta.Nome,
                SaldoAtual = conta.SaldoAtual,
                Lancamentos = lancamentos.Select(l => new LancamentoDto
                {
                    Id = l.Id,
                    Descricao = l.Descricao,
                    Valor = l.Valor,
                    Categoria = l.Categoria?.Nome ?? "Sem Categoria",
                    Data = l.DataVencimento,
                    Tipo = l.Valor >= 0 ? "Receita" : "Despesa" // Lógica simples de visualização
                }).ToList()
            };
        }
    }
}