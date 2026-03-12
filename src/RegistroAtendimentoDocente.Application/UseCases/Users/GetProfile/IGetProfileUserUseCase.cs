using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.GetProfile;

public interface IGetProfileUserUseCase
{
    Task<ResponseShortUserJson> Execute();
}
