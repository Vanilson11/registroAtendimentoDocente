namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel;
public interface IReportExcelServicesUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
