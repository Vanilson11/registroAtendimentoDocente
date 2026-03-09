using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
public interface IAtendimentoWriteOnlyRepository
{
    Task Add(Atendimento atendimento);
    Task Delete(long id);
}
