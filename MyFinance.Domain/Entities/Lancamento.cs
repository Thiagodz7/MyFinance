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
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public bool Pago { get; private set; }


        public Guid ContaId { get; private set; }


        public Conta Conta { get; private set; }

        public Guid CategoriaId { get; private set; }

        public Categoria Categoria { get; private set; }
        public string UserId { get; private set; } = string.Empty;


        public bool EhRecorrente { get; private set; }
        public TipoFrequencia Frequencia { get; private set; }
        public int ParcelaAtual { get; private set; }
        public int TotalParcelas { get; private set; }


        public Guid? GrupoRecorrenciaId { get; private set; }

        public Lancamento(string descricao, decimal valor, DateTime dataVencimento, Guid contaId, Guid categoriaId, bool pago = false)
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

            EhRecorrente = false;
            Frequencia = TipoFrequencia.Nenhuma;
            ParcelaAtual = 1;
            TotalParcelas = 1;
            Pago = pago;
        }

        public void MarcarComoPago() => Pago = true;
        public void AlterarStatusPagamento(bool pago) => Pago = pago;

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
