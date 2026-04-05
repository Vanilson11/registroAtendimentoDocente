using RegistroAtendimentoDocente.Communication.Requests;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Update;
public interface IUpdateConsultationUseCase
{
    Task Execute(RequestConsultationJson request, long id);
}
