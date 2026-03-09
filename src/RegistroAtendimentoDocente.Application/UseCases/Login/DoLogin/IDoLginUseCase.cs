using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Login.DoLogin;
public interface IDoLginUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestDoLoginJson request);
}
