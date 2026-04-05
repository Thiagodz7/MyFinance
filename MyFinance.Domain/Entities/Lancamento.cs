using MyFinance.Domain.Interfaces;

namespace MyFinance.Domain.Entities
{
    public enum TipoFrequencia
    {
        Nenhuma = 0,
        Semanal = 1,
        Mensal = 2,
        Anual = 3
    }
    public class Lancamento : BaseEntity, IEntityComDono
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
        public string UserId { get; private set; } = string.Empty;

        // --- [NOVOS CAMPOS DE RECORRÊNCIA] ---
        public bool EhRecorrente { get; private set; }
        public TipoFrequencia Frequencia { get; private set; }
        public int ParcelaAtual { get; private set; }
        public int TotalParcelas { get; private set; }

        // Agrupa os lançamentos para edição/exclusão em lote no futuro
        public Guid? GrupoRecorrenciaId { get; private set; }


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

            if (categoriaId == Guid.Empty)
                throw new Exception("Categioria inválida");

            Descricao = descricao;
            Valor = valor;
            DataVencimento = dataVencimento;
            ContaId = contaId;
            CategoriaId = categoriaId;
            Pago = false;

            // Valores padrão para lançamentos normais
            EhRecorrente = false;
            Frequencia = TipoFrequencia.Nenhuma;
            ParcelaAtual = 1;
            TotalParcelas = 1;
        }

        public void MarcarComoPago() => Pago = true;

        public void Atualizar(string descricao, decimal valor, DateTime dtVencimento)
        {
            Descricao = descricao;
            Valor = valor;
            DataVencimento = dtVencimento;
        }

        public void AssociarUsuario(string userId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = userId;
            }
        }

        // --- [NOVO MÉTODO] ---
        // Configura este lançamento como parte de uma assinatura/recorrência
        public void ConfigurarRecorrencia(TipoFrequencia frequencia, int parcelaAtual, int totalParcelas, Guid grupoRecorrenciaId)
        {
            EhRecorrente = true;
            Frequencia = frequencia;
            ParcelaAtual = parcelaAtual;
            TotalParcelas = totalParcelas;
            GrupoRecorrenciaId = grupoRecorrenciaId;
        }
    }
}
