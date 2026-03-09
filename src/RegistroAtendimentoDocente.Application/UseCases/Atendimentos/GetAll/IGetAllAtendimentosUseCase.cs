using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetAll;
public interface IGetAllAtendimentosUseCase
{
    Task<ResponseAtendimentosJson> Execute();
}
