using MediatR;

namespace MyFinance.Application.Queries
{
    public class ExportarExtratoQuery : IRequest<(byte[] Conteudo, string NomeArquivo)>
    {
        public Guid ContaId { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }

        public ExportarExtratoQuery(Guid contaId, int mes, int ano)
        {
            ContaId = contaId;
            Mes = mes;
            Ano = ano;
        }
    }
}