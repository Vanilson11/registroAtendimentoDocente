using RegistroAtendimentoDocente.Communication.Responses;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.GetById;
public interface IGetConsultationByIdUseCase
{
    Task<ResponseConsultationJson> Execute(long id);
}
