using RegistroAtendimentoDocente.Domain.Entities;

namespace WeApi.Test.Resources;
public class AtendimentoIdentityManager
{
    private readonly Atendimento _atendimento;

    public AtendimentoIdentityManager(Atendimento atendimento)
    {
        _atendimento = atendimento;
    }

    public long GetId() => _atendimento.Id;
}
