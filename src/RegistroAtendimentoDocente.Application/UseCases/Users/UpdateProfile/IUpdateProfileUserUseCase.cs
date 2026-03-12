using RegistroAtendimentoDocente.Communication.Requests;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.UpdateProfile;

public interface IUpdateProfileUserUseCase
{
    Task Execute(RequestUpdateUserProfileJson request);
}
