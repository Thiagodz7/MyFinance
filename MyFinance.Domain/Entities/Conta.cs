namespace MyFinance.Domain.Entities
{
    public class Conta : BaseEntity
    {
        public string Nome { get; private set; }
        public string Banco { get; private set; }
        public decimal SaldoAtual { get; private set; }

        // Construtor para criar conta nova
        public Conta(string nome, decimal saldoInicial, string banco)
        {
            Nome = nome;
            SaldoAtual = saldoInicial;
            Banco = banco;
        }

        // [CORREÇÃO AQUI] Adicione este construtor vazio para o EF Core
        protected Conta() { }

        // Método Blindado: A única forma de mudar o saldo é por aqui
        public void AtualizarSaldo(decimal valorLancamento)
        {
            // Se o lançamento for positivo (Receita), soma.
            // Se for negativo (Despesa), subtrai.
            SaldoAtual += valorLancamento;
        }
    }
}