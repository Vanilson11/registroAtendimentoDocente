using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportByCoordinator;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportExcelConsultations;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Pdf.ReportPdfByCoordinator;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Pdf.ReportPdfConsultationsUseCase;
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
    [Authorize(Roles = Roles.COORDINATOR)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetReportConsultationsExcel(
        [FromServices] IReportExcelConsultationsUseCase useCase,
        [FromQuery] DateOnly month
        )
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "Report.xlsx");

        return NoContent();
    }

    [HttpGet]
    [Route("excel/coordinator/{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportConsultationsExcelByCoordinator(
        [FromServices] IReportExcelByCoordinatorUseCase useCase,
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
    [Authorize(Roles = Roles.COORDINATOR)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetReportConsultationsPdf(
        [FromServices] IReportPdfConsultationsUseCase useCase,
        [FromQuery] DateOnly month
        )
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

        return NoContent();
    }

    [HttpGet]
    [Route("pdf/coordinator/{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportConsultationsPdfByCoordinator(
        [FromServices] IReportPdfByCoordinatorUseCase useCase,
        [FromRoute] long id
        )
    {
        byte[] file = await useCase.Execute(id);

        if (file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Pdf, "Report.pdf");
        }

        return NoContent();
    }
}
