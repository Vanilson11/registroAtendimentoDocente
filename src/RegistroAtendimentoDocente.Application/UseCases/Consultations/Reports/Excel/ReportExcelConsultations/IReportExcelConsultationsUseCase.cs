namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportExcelConsultations;
public interface IReportExcelConsultationsUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
