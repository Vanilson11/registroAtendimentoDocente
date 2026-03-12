using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories.Users;
public class ReadOnlyUsersRepositoryBuilder
{
    private readonly Mock<IReadOnlyUsersRepository> _repository;

    public ReadOnlyUsersRepositoryBuilder()
    {
        _repository = new Mock<IReadOnlyUsersRepository>();
    }

    public ReadOnlyUsersRepositoryBuilder ExistActiveUserWithEmail(string? email)
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

    public ReadOnlyUsersRepositoryBuilder GetById(User? user)
    {
        if(user is not null)
        {
            _repository.Setup(readOnlyUsers => readOnlyUsers.GetById(It.IsAny<long>())).ReturnsAsync(user);
        }
        
        return this;
    }

    public ReadOnlyUsersRepositoryBuilder GetAll(List<User> users)
    {
        _repository.Setup(readOnlyUsers => readOnlyUsers.GetAll()).ReturnsAsync(users);

        return this;
    }
    public IReadOnlyUsersRepository Build()
    {
        return _repository.Object;
    }
}
