using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.GetById;
public interface IGetAtendimentoByIdUseCase
{
    Task<ResponseAtendimentoJson> Execute(int id);
}
