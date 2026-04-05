using Moq;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;

namespace CommonTestUtilities.Repositories.Consultations;

public class ConsultationsWriteOnlyRepositoryBuilder
{
    public static IConsultationsWriteOnlyRepository Build()
    {
        var mock = new Mock<IConsultationsWriteOnlyRepository>();

        return mock.Object;
    }
}
