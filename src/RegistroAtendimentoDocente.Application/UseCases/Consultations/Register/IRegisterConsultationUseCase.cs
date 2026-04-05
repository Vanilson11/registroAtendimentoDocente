using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Register;
public interface IRegisterConsultationUseCase
{
    Task<ResponseRegisterConsultationJson> Execute(RequestConsultationJson request);
}
