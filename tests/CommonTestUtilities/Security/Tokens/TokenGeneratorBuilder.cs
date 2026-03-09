using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Security.Tokens;

namespace CommonTestUtilities.Security.Tokens;
public class TokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(tokenGenerator => tokenGenerator.Genetate(It.IsAny<User>())).Returns("token_teste");

        return mock.Object;
    }
}
