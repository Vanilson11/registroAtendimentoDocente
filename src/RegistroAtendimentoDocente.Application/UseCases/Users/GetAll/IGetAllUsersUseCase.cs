using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.GetAll;

public interface IGetAllUsersUseCase
{
    Task<ResponseUsersJson> Execute();
}
