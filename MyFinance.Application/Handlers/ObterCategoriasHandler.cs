using MediatR;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class ObterCategoriasHandler : IRequestHandler<ObterCategoriasQuery, IEnumerable<CategoriaDto>>
    {
        private readonly ICategoriaRepository _repository;

        public ObterCategoriasHandler(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoriaDto>> Handle(ObterCategoriasQuery request, CancellationToken cancellationToken)
        {
            var categorias = await _repository.GetAllAsync();

            var listaCategoriasDto = categorias.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Tipo = c.Tipo.ToString(),
                Ativo = c.Ativo 
            });

             if (request.ApenasAtivos)
             {
                 listaCategoriasDto.Where(c => c.Ativo);
             }

             return listaCategoriasDto;
        }
    }
}