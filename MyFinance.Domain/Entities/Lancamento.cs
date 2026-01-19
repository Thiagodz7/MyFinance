namespace MyFinance.Domain.Entities
{
    public class Lancamento : BaseEntity
    {
        // Propriedades "private set" para garantir encapsulamento. 
        // Ninguém muda o valor "de fora" sem passar pelo construtor ou método.
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public bool Pago { get; private set; }

        // [NOVO] Chave Estrangeira (Foreign Key)
        public Guid ContaId { get; private set; }

        // [NOVO] Propriedade de Navegação (Para o EF trazer a conta junto se precisar)
        public Conta Conta { get; private set; }

        public Guid CategoriaId { get; private set; }

        public Categoria Categoria { get; private set; }

        //Construtor após a Refatoração para incluir ContaId
        // [ATUALIZADO] Adicione contaId no construtor
        public Lancamento(string descricao, decimal valor, DateTime dataVencimento, Guid contaId, Guid categoriaId)
        {
            if (string.IsNullOrEmpty(descricao))
                throw new Exception("Descrição é obrigatória");

            if (valor == 0)
                throw new Exception("O valor não pode ser zero");

            if (contaId == Guid.Empty)
                throw new Exception("Conta inválida");

            if(categoriaId == Guid.Empty)
                throw new Exception("Categioria inválida");

            Descricao = descricao;
            Valor = valor;
            DataVencimento = dataVencimento;
            ContaId = contaId;
            CategoriaId = categoriaId;
            Pago = false;
        }   


        public void MarcarComoPago() => Pago = true;


        //Construtor antes da Refatoração
        //public Lancamento(string descricao, decimal valor, DateTime dataVencimento)
        //{
        //    // Validação simples (Regra de Domínio)
        //    if (string.IsNullOrEmpty(descricao))
        //        throw new Exception("Descrição é obrigatória");

        //    if (valor == 0)
        //        throw new Exception("O valor não pode ser zero");

        //    Descricao = descricao;
        //    Valor = valor;
        //    DataVencimento = dataVencimento;
        //    Pago = false;
        //}
    }
}
