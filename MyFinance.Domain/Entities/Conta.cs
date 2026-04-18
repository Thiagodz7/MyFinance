using MyFinance.Domain.Interfaces;
using MyFinance.Shared.Enums;

namespace MyFinance.Domain.Entities
{
    public class Conta : BaseEntity, IEntityComDono
    {
        public string Nome { get; private set; }
        public string Banco { get; private set; }
        public decimal SaldoAtual { get; private set; }
        public bool Ativo { get; private set; }
        public string UserId { get; private set; } = string.Empty;
        public TipoConta Tipo { get; private set; } = TipoConta.Corrente;

        // Construtor para criar conta nova
        public Conta(string nome, decimal saldoInicial, string banco, TipoConta tipo)
        {
            Nome = nome;
            SaldoAtual = saldoInicial;
            Banco = banco;
            Ativo = true;
            Tipo = tipo;
        }

        protected Conta() { }

        public void AtualizarSaldo(decimal valorLancamento)
        {
            SaldoAtual += valorLancamento;
        }
        public void Atualizar(string nome, string banco, TipoConta tipo)
        {
            Nome = nome;
            Banco = banco;
            Tipo = tipo;
        }
        public void AlterarStatus(bool ativo)
        {
            Ativo = ativo;
        }

        public void AssociarUsuario(string userId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = userId;
            }
        }

        public void MarcarComoSimulacao()
        {
            Tipo = TipoConta.Simulacao;
        }
    }
}