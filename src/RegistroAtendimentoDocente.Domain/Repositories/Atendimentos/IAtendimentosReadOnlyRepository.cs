using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
public interface IAtendimentosReadOnlyRepository
{
    Task<List<Atendimento>> GetAll(User user);
    Task<Atendimento?> GetById(User user, long id);

    Task<List<Atendimento>> FilterServicesByMonth(User user, DateOnly month);
}
