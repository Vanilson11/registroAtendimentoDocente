using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetById;
public interface IGetAtendimentoByIdUseCase
{
    Task<ResponseAtendimentoJson> Execute(long id);
}
