namespace MyFinance.Application.DTOs
{
    public class CategoriaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "Receita" ou "Despesa"
        public int TipoSelecionado { get; set; }
        public bool Ativo { get; set; }
    }  
}