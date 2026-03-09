using Moq;
using RegistroAtendimentoDocente.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories.Users;
public class WriteOnlyUsersRepositoryBuilder
{
    public static IWriteOnlyUsersRepository Build()
    {
        var mock = new Mock<IWriteOnlyUsersRepository>();

        return mock.Object;
    }
}
