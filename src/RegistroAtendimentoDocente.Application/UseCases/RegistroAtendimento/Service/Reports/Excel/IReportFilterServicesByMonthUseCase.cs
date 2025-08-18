namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Excel;
public interface IReportFilterServicesByMonthUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
