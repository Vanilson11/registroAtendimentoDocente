using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Delete;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.GetById;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Register;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Update;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Enums;

namespace RegistroAtendimentoDocente.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ConsultationController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Roles.COORDINATOR)]
    [ProducesResponseType(typeof(ResponseConsultationsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllConsultations([FromServices] IGetAllConsultationsUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = Roles.COORDINATOR)]
    [ProducesResponseType(typeof(ResponseConsultationJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetConsultationById([FromServices] IGetConsultationByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = Roles.COORDINATOR)]
    [ProducesResponseType(typeof(ResponseRegisterConsultationJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Register([FromServices] IRegisterConsultationUseCase useCase, [FromBody] RequestConsultationJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = Roles.COORDINATOR)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateConsultation([FromServices] IUpdateConsultationUseCase useCase,
        [FromBody] RequestConsultationJson request,
        [FromRoute] long id)
    {
        await useCase.Execute(request, id);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Roles.COORDINATOR)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteConsultation([FromServices] IDeleteConsultationUseCase useCase, [FromRoute] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
