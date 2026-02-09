using Microsoft.AspNetCore.Identity;

namespace MyFinance.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty; 

        public string CNPJ { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
