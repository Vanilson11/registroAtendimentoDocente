using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Excel;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Pdf;
using System.Net.Mime;

namespace RegistroAtendimentoDocente.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetReportServicesExcel(
        [FromServices] IReportFilterServicesByMonthUseCase useCase,
        [FromHeader] DateOnly month
        )
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "Report.xlsx");

        return NoContent();
    }

    [HttpGet("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetReportServicesPdf(
        [FromServices] IReportPdfServicesUseCase useCase,
        [FromHeader] DateOnly month
        )
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

        return NoContent();
    }
}
