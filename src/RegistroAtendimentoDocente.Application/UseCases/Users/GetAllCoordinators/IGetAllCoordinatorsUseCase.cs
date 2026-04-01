using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.GetAllCoordinators;

public interface IGetAllCoordinatorsUseCase
{
    Task<ResponseUsersJson> Execute();
}
