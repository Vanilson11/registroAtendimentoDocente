namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel.GetReportByCoordenador;

public interface IReportExcelByCoordenadorUseCase
{
    Task<byte[]> Execute(long id);
}
