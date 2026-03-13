using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel.GetReportByCoordenador;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Pdf;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Enums;
using System.Net.Mime;

namespace RegistroAtendimentoDocente.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ReportsController : ControllerBase
{
    [HttpGet("excel")]
    [Authorize(Roles = Roles.COORDENADOR)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetReportServicesExcel(
        [FromServices] IReportExcelServicesUseCase useCase,
        [FromQuery] DateOnly month
        )
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "Report.xlsx");

        return NoContent();
    }

    [HttpGet]
    [Route("excel/coordenador/{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportServicesExcelByCoordenador(
        [FromServices] IReportExcelByCoordenadorUseCase useCase,
        [FromRoute] long id
        )
    {
        byte[] file = await useCase.Execute(id);

        if(file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Octet, "Report.xlsx");
        }

        return NoContent();
    }

    [HttpGet("pdf")]
    [Authorize(Roles = Roles.COORDENADOR)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetReportServicesPdf(
        [FromServices] IReportPdfServicesUseCase useCase,
        [FromQuery] DateOnly month
        )
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

        return NoContent();
    }
}
