using MediatR;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;
using MyFinance.Domain.Interfaces;

public class ObterLancamentoHandler : IRequestHandler<ObterLancamentoPorIdQuery, LancamentoDto?>
{
    private readonly ILancamentoRepository _repository;

    public ObterLancamentoHandler(ILancamentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<LancamentoDto?> Handle(ObterLancamentoPorIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity == null) return null;

        // Mapeamento Manual (ou use AutoMapper se tiver)
        return new LancamentoDto
        {
            Id = entity.Id,
            Descricao = entity.Descricao,
            Valor = entity.Valor,
            Data = entity.DataVencimento,
            ContaId = entity.ContaId,
            CategoriaId = entity.CategoriaId,
            // Preencha o resto conforme seu DTO
        };
    }
}