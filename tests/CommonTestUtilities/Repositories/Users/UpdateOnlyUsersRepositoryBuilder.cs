using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories.Users;

public class UpdateOnlyUsersRepositoryBuilder
{
    private readonly Mock<IUpdateOnlyUsersRepository> _mock;

    public UpdateOnlyUsersRepositoryBuilder()
    {
        _mock = new Mock<IUpdateOnlyUsersRepository>();
    }

    public UpdateOnlyUsersRepositoryBuilder GetById(User? user)
    {
        if(user is not null)
        {
            _mock.Setup(repository => repository.GetById(It.IsAny<long>())).ReturnsAsync(user);
        }

        return this;
    }

    public IUpdateOnlyUsersRepository Build() { return _mock.Object; }
}
