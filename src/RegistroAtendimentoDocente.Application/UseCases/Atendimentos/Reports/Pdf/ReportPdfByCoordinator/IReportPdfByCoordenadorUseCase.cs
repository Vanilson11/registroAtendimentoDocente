namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Pdf.ReportPdfByCoordinator;

public interface IReportPdfByCoordenadorUseCase
{
    Task<byte[]> Execute(long id);
}
