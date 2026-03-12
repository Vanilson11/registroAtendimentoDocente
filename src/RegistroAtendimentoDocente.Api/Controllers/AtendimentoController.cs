using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Delete;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetById;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Register;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Update;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Enums;

namespace RegistroAtendimentoDocente.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class AtendimentoController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Roles.COORDENADOR)]
    [ProducesResponseType(typeof(ResponseAtendimentosJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllAtendimentos([FromServices] IGetAllAtendimentosUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = Roles.COORDENADOR)]
    [ProducesResponseType(typeof(ResponseAtendimentoJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAtendimentoById([FromServices] IGetAtendimentoByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = Roles.COORDENADOR)]
    [ProducesResponseType(typeof(ResponseRegisterAtendimentoJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Register([FromServices] IRegisterAtendimentoUseCase useCase, [FromBody] RequestAtendimentoJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = Roles.COORDENADOR)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAtendimento([FromServices] IUpdateAtendimentoUseCase useCase,
        [FromBody] RequestAtendimentoJson request,
        [FromRoute] long id)
    {
        await useCase.Execute(request, id);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = $"{Roles.COORDENADOR}, {Roles.ADMIN}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAtendimento([FromServices] IDeleteAtendimentosUseCase useCase, [FromRoute] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
