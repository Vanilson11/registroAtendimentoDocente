using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
public interface IAtendimentoWriteOnlyRepository
{
    Task Add(Atendimento atendimento);

    /// <summary>
    /// The function returns TRUE if deletion was successfull otherwise FALSE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Delete(int id);
}
