using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
public interface IAtendimentoUpdateOnlyUseCase
{
    Task<Atendimento?> GetById(int id);

    void Update(Atendimento atendimento);
}
