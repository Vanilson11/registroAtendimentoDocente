using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Register;
public interface IRegisterAtendimentoUseCase
{
    Task<ResponseRegisterAtendimentoJson> Execute(RequestAtendimentoJson request);
}
