using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
public interface IAtendimentosReadOnlyRepository
{
    Task<List<Atendimento>> GetAll();
    Task<Atendimento?> GetById(int id);

    Task<List<Atendimento>> FilterServicesByMonth(DateOnly month);
}
