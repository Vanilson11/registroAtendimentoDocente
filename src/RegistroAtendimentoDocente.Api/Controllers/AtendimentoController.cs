using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Delete;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetById;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Register;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Update;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class AtendimentoController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseAtendimentosJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAtendimentos([FromServices] IGetAllAtendimentosUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseAtendimentoJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAtendimentoById([FromServices] IGetAtendimentoByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterAtendimentoJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterAtendimentoUseCase useCase, [FromBody] RequestAtendimentoJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAtendimento([FromServices] IUpdateAtendimentoUseCase useCase,
        [FromBody] RequestAtendimentoJson request,
        [FromRoute] long id)
    {
        await useCase.Execute(request, id);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAtendimento([FromServices] IDeleteAtendimentosUseCase useCase, [FromRoute] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
