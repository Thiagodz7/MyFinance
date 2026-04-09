using MyFinance.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Shared.DTOs
{
    public class PerfilUsuarioDto
    {
        // Aba 1: Dados Pessoais
        public string NomeCompleto { get; set; } = string.Empty;
        public TipoOcupacao Ocupacao { get; set; } = TipoOcupacao.NaoInformado;

        // Aba 2: Orçamento e Metas
        public decimal? TetoGastosMensal { get; set; }

        // Aba 3: SaaS e Assinatura
        public string PlanoAtual { get; set; } = "Free";
        public string TokenAssinatura { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

        public bool NotificarEmail { get; set; }
        public bool NotificarTelefone { get; set; }
        public bool NotificarPush { get; set; }
        public bool NotificarWhatsapp { get; set; }
    }
}
