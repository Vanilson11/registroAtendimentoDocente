using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Register;
public interface IRegisterAtendimentoUseCase
{
    Task<ResponseRegisterAtendimentoJson> Execute(RequestAtendimentoJson request);
}
