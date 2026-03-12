using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroAtendimentoDocente.Application.UseCases.Users.DeleteProfile;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetById;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetProfile;
using RegistroAtendimentoDocente.Application.UseCases.Users.Register;
using RegistroAtendimentoDocente.Application.UseCases.Users.Update;
using RegistroAtendimentoDocente.Application.UseCases.Users.UpdateProfile;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Enums;

namespace RegistroAtendimentoDocente.Api.Controllers;

[Route("[controller]")]
[ApiController]
//change password
//ADMIN:
//delete user by id
public class UsersController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseUsersJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllUsersUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetUserByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }


    [HttpGet("get-profile")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseShortUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile([FromServices] IGetProfileUserUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request
        )
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile(
        [FromServices] IUpdateProfileUserUseCase useCase,
        [FromBody] RequestUpdateUserProfileJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateUserUseCase useCase,
        [FromBody] RequestUpdateUserJson request,
        [FromRoute] long id
        )
    {
        await useCase.Execute(request, id);

        return NoContent(); 
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteProfile([FromServices] IDeleteProfileUserUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }
}
