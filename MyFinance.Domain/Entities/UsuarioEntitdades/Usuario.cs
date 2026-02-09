namespace MyFinance.Domain.Entities.UsuarioEntitdades
{
    public class Usuario : BaseEntity
    {
        public string Email { get; private set; } = string.Empty;
        public string CNPJ { get; private set; } = string.Empty;
        public string CPF { get; private set; } = string.Empty;
        public DateTime dtNascimento { get; private set; }
        public bool Ativo { get; private set; }
    }
}
