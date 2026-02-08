namespace MyFinance.Domain.Entities
{
    public class Conta : BaseEntity
    {
        public string Nome { get; private set; }
        public string Banco { get; private set; }
        public decimal SaldoAtual { get; private set; }
        public bool Ativo { get; private set; }

        // Construtor para criar conta nova
        public Conta(string nome, decimal saldoInicial, string banco)
        {
            Nome = nome;
            SaldoAtual = saldoInicial;
            Banco = banco;
            Ativo = true;
        }

        protected Conta() { }

        public void AtualizarSaldo(decimal valorLancamento)
        {
            SaldoAtual += valorLancamento;
        }
        public void Atualizar(string nome, string banco)
        {
            Nome = nome;
            Banco = banco;
        }
        public void AlterarStatus(bool ativo)
        {
            Ativo = ativo;
        }
    }
}