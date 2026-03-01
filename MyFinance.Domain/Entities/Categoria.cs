using MyFinance.Domain.Interfaces;

namespace MyFinance.Domain.Entities
{
    public class Categoria : BaseEntity, IEntityComDono
    {
        public string Nome { get; private set; } = string.Empty;
        public TipoCategoria Tipo { get; set; }
        public bool Ativo { get; private set; }
        public string UserId { get; private set; } = string.Empty;
        public Categoria(string nome, TipoCategoria tipo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("O nome da categoria é obrigatório.");

            if((int)tipo < 0 || (int)tipo > 1)
                throw new Exception("Tipo de categoria inválido.");

            Nome = nome;
            Tipo = tipo;
            Ativo = true;
        }

        public void Atualizar(string nome, TipoCategoria tipo)
        {
            Nome = nome;
            Tipo = tipo;
        }

        public void AlterarStatus(bool ativo)
        {
            Ativo = ativo;
        }

        public void AssociarUsuario(string userId)
        {
            throw new NotImplementedException();
        }

        protected Categoria() { } 
    }

    public enum TipoCategoria
    {
        Receita,
        Despesa
    }
}
