namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Pdf;
public interface IReportPdfServicesUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
