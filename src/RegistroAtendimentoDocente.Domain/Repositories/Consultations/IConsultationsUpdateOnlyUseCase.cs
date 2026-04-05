using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Consultations;
public interface IConsultationsUpdateOnlyUseCase
{
    Task<Consultation?> GetById(User user, long id);

    void Update(Consultation consultation);
}
