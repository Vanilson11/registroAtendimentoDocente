using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Consultations;
public interface IConsultationsReadOnlyRepository
{
    Task<List<Consultation>> GetAll(User user);
    Task<Consultation?> GetById(User user, long id);

    Task<List<Consultation>> FilterConsultationsByMonth(User user, DateOnly month);
}
