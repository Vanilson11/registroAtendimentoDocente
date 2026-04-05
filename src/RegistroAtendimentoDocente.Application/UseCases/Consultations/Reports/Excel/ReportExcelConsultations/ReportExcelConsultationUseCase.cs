using ClosedXML.Excel;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Reports;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportExcelConsultations;
public class ReportExcelConsultationUseCase : IReportExcelConsultationsUseCase
{
    private readonly IConsultationsReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public ReportExcelConsultationUseCase(IConsultationsReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();

        var consultations = await _repository.FilterConsultationsByMonth(loggedUser, month);

        if (consultations.Count == 0)
            return [];

        var workbook = new XLWorkbook();

        ConfigsGeneralWorkbook(workbook, loggedUser.Name);

        var worksheet = workbook.AddWorksheet(month.ToString("Y"));

        InsertHeader(worksheet);

        InsertValeusOnCells(consultations, worksheet);

        ConfigWidthColumns(worksheet);

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void ConfigsGeneralWorkbook(XLWorkbook workbook, string authorName)
    {
        workbook.Author = authorName;
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportsMessages.TEACHER;
        worksheet.Cell("B1").Value = ResourceReportsMessages.SUBJECT;
        worksheet.Cell("C1").Value = ResourceReportsMessages.DATE;
        worksheet.Cell("D1").Value = ResourceReportsMessages.RECOMMENDATIONS;
        worksheet.Cell("E1").Value = ResourceReportsMessages.OBSERVATION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#4DA8DA");
        worksheet.Cells("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }

    private void InsertValeusOnCells(List<Consultation> services, IXLWorksheet worksheet)
    {
        var raw = 2;
        foreach(var service in services)
        {
            worksheet.Cell($"A{raw}").Value = service.Teacher;
            worksheet.Cell($"B{raw}").Value = service.Subject;
            worksheet.Cell($"C{raw}").Value = service.Date;
            worksheet.Cell($"D{raw}").Value = service.Recommendations;
            worksheet.Cell($"E{raw}").Value = service.Observation;

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
