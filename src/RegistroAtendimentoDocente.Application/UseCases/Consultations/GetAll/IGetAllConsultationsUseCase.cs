using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.GetAll;
public interface IGetAllConsultationsUseCase
{
    Task<ResponseConsultationsJson> Execute();
}
