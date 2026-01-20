namespace MyFinance.Domain.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; private set; } = string.Empty;
        public TipoCategoria Tipo { get; set; }

        public Categoria(string nome, TipoCategoria tipo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("O nome da categoria é obrigatório.");

            if((int)tipo < 0 || (int)tipo > 1)
                throw new Exception("Tipo de categoria inválido.");

            Nome = nome;
            Tipo = tipo;
        }

        public void Atualizar(string nome, TipoCategoria tipo)
        {
            Nome = nome;
            Tipo = tipo;
        }

        protected Categoria() { } // Construtor protegido para EF Core
    }

    public enum TipoCategoria
    {
        Receita,
        Despesa
    }
}
