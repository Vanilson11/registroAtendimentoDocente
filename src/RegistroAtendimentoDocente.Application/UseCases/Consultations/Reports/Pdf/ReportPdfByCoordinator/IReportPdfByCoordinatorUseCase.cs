namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Pdf.ReportPdfByCoordinator;

public interface IReportPdfByCoordinatorUseCase
{
    Task<byte[]> Execute(long id);
}
