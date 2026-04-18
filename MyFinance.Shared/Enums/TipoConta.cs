using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Shared.Enums
{
    public enum TipoConta
    {
        Corrente = 1,
        Poupanca = 2,
        Investimento = 3,
        CartaoCredito = 4,
        Simulacao = 99 // O nosso ambiente "Sandbox"
    }
}
