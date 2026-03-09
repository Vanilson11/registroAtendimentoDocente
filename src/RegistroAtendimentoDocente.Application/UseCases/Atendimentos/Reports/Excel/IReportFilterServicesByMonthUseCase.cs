namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel;
public interface IReportFilterServicesByMonthUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
