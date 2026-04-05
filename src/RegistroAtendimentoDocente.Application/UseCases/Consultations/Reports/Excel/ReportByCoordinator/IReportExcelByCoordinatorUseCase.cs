namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportByCoordinator;

public interface IReportExcelByCoordinatorUseCase
{
    Task<byte[]> Execute(long id);
}
