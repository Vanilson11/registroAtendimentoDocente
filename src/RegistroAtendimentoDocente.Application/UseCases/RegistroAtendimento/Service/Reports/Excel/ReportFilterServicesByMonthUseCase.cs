using ClosedXML.Excel;
using PdfSharp.Fonts;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Pdf.Fonts;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Reports;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Excel;
public class ReportFilterServicesByMonthUseCase : IReportFilterServicesByMonthUseCase
{
    private readonly IAtendimentosReadOnlyRepository _repository;

    public ReportFilterServicesByMonthUseCase(IAtendimentosReadOnlyRepository repository)
    {
        _repository = repository;
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var services = await _repository.FilterServicesByMonth(month);

        if (services.Count == 0)
            return [];

        var workbook = new XLWorkbook();

        ConfigsGeneralWorkbook(workbook);

        var worksheet = workbook.AddWorksheet(month.ToString("Y"));

        InsertHeader(worksheet);

        InsertValeusOnCells(services, worksheet);

        ConfigWidthColumns(worksheet);

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void ConfigsGeneralWorkbook(XLWorkbook workbook)
    {
        workbook.Author = "Vanilson Sousa";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportsMessages.TEACHER;
        worksheet.Cell("B1").Value = ResourceReportsMessages.SUBJECT;
        worksheet.Cell("C1").Value = ResourceReportsMessages.DATE;
        worksheet.Cell("D1").Value = ResourceReportsMessages.FOWARDING;
        worksheet.Cell("E1").Value = ResourceReportsMessages.OBSERVATION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#4DA8DA");
        worksheet.Cells("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }

    private void InsertValeusOnCells(List<Atendimento> services, IXLWorksheet worksheet)
    {
        var raw = 2;
        foreach(var service in services)
        {
            worksheet.Cell($"A{raw}").Value = service.Docente;
            worksheet.Cell($"B{raw}").Value = service.Assunto;
            worksheet.Cell($"C{raw}").Value = service.Data;
            worksheet.Cell($"D{raw}").Value = service.Encaminhamento;
            worksheet.Cell($"E{raw}").Value = service.Observacao;

            raw++;
        }
    }

    private void ConfigWidthColumns(IXLWorksheet worksheet)
    {
        worksheet.Column("A").Width = 30;
        worksheet.Column("B").Width = 50;
        worksheet.Column("C").Width = 20;
        worksheet.Column("D").Width = 50;
        worksheet.Column("E").Width = 50;
    }
}
