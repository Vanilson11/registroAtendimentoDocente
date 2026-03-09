using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;

namespace CommonTestUtilities;
public class ReadOnlyUsersRepositoryBuilder
{
    private readonly Mock<IReadOnlyUsersRepository> _repository;

    public ReadOnlyUsersRepositoryBuilder()
    {
        _repository = new Mock<IReadOnlyUsersRepository>();
    }

    public ReadOnlyUsersRepositoryBuilder ExistActiveUserWithEmail(string email)
    {
        if(string.IsNullOrWhiteSpace(email) == false)
        {
            _repository.Setup(readOnlyUsers => readOnlyUsers.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
        }

        return this;
    }

    public ReadOnlyUsersRepositoryBuilder GetByEmail(User user)
    {
        if(user != null)
        {
            _repository.Setup(readOnlyUsers => readOnlyUsers.GetByEmail(user.Email)).ReturnsAsync(user);
        }

        return this;
    }
    public IReadOnlyUsersRepository Build()
    {
        return _repository.Object;
    }
}
