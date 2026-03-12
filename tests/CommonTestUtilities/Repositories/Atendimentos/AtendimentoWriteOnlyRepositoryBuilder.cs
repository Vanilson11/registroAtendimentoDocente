using Moq;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;

namespace CommonTestUtilities.Repositories.Atendimentos;

public class AtendimentoWriteOnlyRepositoryBuilder
{
    public static IAtendimentoWriteOnlyRepository Build()
    {
        var mock = new Mock<IAtendimentoWriteOnlyRepository>();

        return mock.Object;
    }
}
