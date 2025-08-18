using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Delete;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.GetById;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Register;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Update;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AtendimentoController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseAtendimentosJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllAtendimentos([FromServices] IGetAllAtendimentosUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Atendimentos.Count != 0) return Ok(response);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseAtendimentoJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseAtendimentoErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAtendimentoById([FromServices] IGetAtendimentoByIdUseCase useCase, [FromRoute] int id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterAtendimentoJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseAtendimentoErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterAtendimentoUseCase useCase, [FromBody] RequestAtendimentoJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseAtendimentoErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseAtendimentoErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAtendimento([FromServices] IUpdateAtendimentoUseCase useCase,
        [FromBody] RequestAtendimentoJson request,
        [FromRoute] int id)
    {
        await useCase.Execute(request, id);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseAtendimentoErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAtendimento([FromServices] IDeleteAtendimentosUseCase useCase, [FromRoute] int id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
