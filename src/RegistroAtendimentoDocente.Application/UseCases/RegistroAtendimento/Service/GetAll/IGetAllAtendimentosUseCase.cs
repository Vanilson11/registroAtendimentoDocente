using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.GetAll;
public interface IGetAllAtendimentosUseCase
{
    Task<ResponseAtendimentosJson> Execute();
}
