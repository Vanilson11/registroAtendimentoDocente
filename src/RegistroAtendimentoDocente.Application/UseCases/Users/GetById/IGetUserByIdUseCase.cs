using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.GetById;

public interface IGetUserByIdUseCase
{
    Task<ResponseUserJson> Execute(long id);
}
