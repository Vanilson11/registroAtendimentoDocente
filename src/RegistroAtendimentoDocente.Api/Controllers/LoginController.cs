using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.Login.DoLogin;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromServices] IDoLginUseCase useCase, [FromBody] RequestDoLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
