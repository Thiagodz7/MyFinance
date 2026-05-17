using ClosedXML.Excel;
using MediatR;
using MyFinance.Application.Queries;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class ExportarExtratoHandler : IRequestHandler<ExportarExtratoQuery, (byte[] Conteudo, string NomeArquivo)>
    {
        private readonly ILancamentoRepository _repository;
        private readonly IContaRepository _contaRepository;

        public ExportarExtratoHandler(ILancamentoRepository repository, IContaRepository contaRepository)
        {
            _repository = repository;
            _contaRepository = contaRepository;
        }

        public async Task<(byte[] Conteudo, string NomeArquivo)> Handle(ExportarExtratoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.GetByIdAsync(request.ContaId);
            var dataInicio = new DateTime(request.Ano, request.Mes, 1);
            var dataFim = dataInicio.AddMonths(1).AddDays(-1);

            var todosLancamentos = await _repository.GetByContaIdAsync(request.ContaId);
            var lancamentos = todosLancamentos
                .Where(l => l.DataVencimento.Date >= dataInicio.Date && l.DataVencimento.Date <= dataFim.Date)
                .OrderBy(l => l.DataVencimento)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Extrato");

                worksheet.Cell(1, 1).Value = "Data";
                worksheet.Cell(1, 2).Value = "Descrição";
                worksheet.Cell(1, 3).Value = "Categoria";
                worksheet.Cell(1, 4).Value = "Valor";
                worksheet.Cell(1, 5).Value = "Situação";

                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Font.FontColor = XLColor.White;
                headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#0F172A"); 
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRow.Height = 24;

                int linha = 2;
                foreach (var l in lancamentos)
                {
                    worksheet.Cell(linha, 1).Value = l.DataVencimento.ToString("dd/MM/yyyy");
                    worksheet.Cell(linha, 2).Value = l.Descricao;
                    worksheet.Cell(linha, 3).Value = l.Categoria?.Nome ?? "-";

                    var cellValor = worksheet.Cell(linha, 4);
                    cellValor.Value = l.Valor;
                    cellValor.Style.NumberFormat.Format = "R$ #,##0.00;[Red]-R$ #,##0.00"; 

                    worksheet.Cell(linha, 5).Value = l.Pago ? "Pago/Recebido" : "Pendente";

                    worksheet.Cell(linha, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(linha, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    if (linha % 2 == 0)
                    {
                        worksheet.Row(linha).Style.Fill.BackgroundColor = XLColor.FromHtml("#F8FAFC"); 
                    }

                    worksheet.Row(linha).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Row(linha).Style.Border.BottomBorderColor = XLColor.FromHtml("#E2E8F0");
                    worksheet.Row(linha).Height = 20;

                    linha++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var nomeArquivo = $"Extrato_{conta?.Nome ?? "Conta"}_{request.Mes:00}_{request.Ano}.xlsx";
                    return (stream.ToArray(), nomeArquivo);
                }
            }
        }
    }
}