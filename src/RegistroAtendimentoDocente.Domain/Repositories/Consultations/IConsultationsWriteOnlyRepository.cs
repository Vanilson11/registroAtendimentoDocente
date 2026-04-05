using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Consultations;
public interface IConsultationsWriteOnlyRepository
{
    Task Add(Consultation consultation);
    Task Delete(long id);
}
