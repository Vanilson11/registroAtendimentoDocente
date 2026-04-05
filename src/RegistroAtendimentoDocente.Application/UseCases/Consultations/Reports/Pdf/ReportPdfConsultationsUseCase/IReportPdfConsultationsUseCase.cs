namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Pdf.ReportPdfConsultationsUseCase;
public interface IReportPdfConsultationsUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
