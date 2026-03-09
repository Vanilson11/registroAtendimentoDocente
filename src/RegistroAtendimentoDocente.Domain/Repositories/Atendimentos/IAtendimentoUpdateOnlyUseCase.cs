using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
public interface IAtendimentoUpdateOnlyUseCase
{
    Task<Atendimento?> GetById(User user, long id);

    void Update(Atendimento atendimento);
}
