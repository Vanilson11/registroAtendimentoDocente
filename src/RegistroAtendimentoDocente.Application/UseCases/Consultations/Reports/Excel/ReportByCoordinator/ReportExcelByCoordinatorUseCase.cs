using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Enums;
using RegistroAtendimentoDocente.Domain.Reports;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportByCoordinator;

public class ReportExcelByCoordinatorUseCase : IReportExcelByCoordinatorUseCase
{
    private readonly IConsultationsReadOnlyRepository _readOnlyConsultationsRepository;
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    private readonly ILoggedUser _loggedUser;
    public ReportExcelByCoordinatorUseCase(
        IConsultationsReadOnlyRepository readOnlyConsultationsRepository,
        IReadOnlyUsersRepository readOnlyUsersRepository,
        ILoggedUser loggedUser
        )
    {
        _readOnlyConsultationsRepository = readOnlyConsultationsRepository;
        _readOnlyUsersRepository = readOnlyUsersRepository;
        _loggedUser = loggedUser;
    }
    public async Task<byte[]> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var user = await _readOnlyUsersRepository.GetById(id);

        if (user is null) throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        if (user.Role is not Roles.COORDINATOR)
            throw new UserNotCoordinatorException(ResourceErrorMessages.USER_IS_NOT_COORDINATOR);

        var consultations = await _readOnlyConsultationsRepository.GetAll(user);

        if (consultations.Count == 0) return [];

        var workbook = new XLWorkbook();

        ConfigsGeneralWorkbook(workbook, loggedUser.Name);

        var worksheet = workbook.AddWorksheet(user.Name);

        InsertHeader(worksheet);

        InsertValeusOnCells(consultations, worksheet, user.Name);

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
        worksheet.Cell("F1").Value = ResourceReportsMessages.COORDINATOR;

        worksheet.Cells("A1:F1").Style.Font.Bold = true;
        worksheet.Cells("A1:F1").Style.Fill.BackgroundColor = XLColor.FromHtml("#4DA8DA");
        worksheet.Cells("A1:F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
    private void InsertValeusOnCells(List<Consultation> services, IXLWorksheet worksheet, string nameCoordenador)
    {
        var raw = 2;
        foreach (var service in services)
        {
            worksheet.Cell($"A{raw}").Value = service.Teacher;
            worksheet.Cell($"B{raw}").Value = service.Subject;
            worksheet.Cell($"C{raw}").Value = service.Date;
            worksheet.Cell($"D{raw}").Value = service.Recommendations;
            worksheet.Cell($"E{raw}").Value = service.Observation;
            worksheet.Cell($"F{raw}").Value = nameCoordenador;

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
        worksheet.Column("F").Width = 50;
    }
}
