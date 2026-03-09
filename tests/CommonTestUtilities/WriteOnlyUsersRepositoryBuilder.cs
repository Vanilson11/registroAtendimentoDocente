using Moq;
using RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;

namespace CommonTestUtilities;
public class WriteOnlyUsersRepositoryBuilder
{
    public static IWriteOnlyUsersRepository Build()
    {
        var mock = new Mock<IWriteOnlyUsersRepository>();

        return mock.Object;
    }
}
