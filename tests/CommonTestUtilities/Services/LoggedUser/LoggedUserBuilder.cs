using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;

namespace CommonTestUtilities.Services.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        if(user is not null)
        {
            mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);
        }

        return mock.Object;
    }
}
