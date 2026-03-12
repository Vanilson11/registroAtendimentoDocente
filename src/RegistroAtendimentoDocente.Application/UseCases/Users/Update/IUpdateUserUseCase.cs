using RegistroAtendimentoDocente.Communication.Requests;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request, long id);
}
