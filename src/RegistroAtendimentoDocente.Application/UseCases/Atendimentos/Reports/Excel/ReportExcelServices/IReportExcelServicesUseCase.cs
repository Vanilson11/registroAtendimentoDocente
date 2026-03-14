namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel.ReportExcelServices;
public interface IReportExcelServicesUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
