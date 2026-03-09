namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Pdf;
public interface IReportPdfServicesUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
