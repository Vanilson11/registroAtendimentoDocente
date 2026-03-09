using Moq;
using RegistroAtendimentoDocente.Domain.Security.Criptography;

namespace CommonTestUtilities;
public class PasswordEncripterBuilder
{
    private readonly Mock<IPasswordEncripter> _encripter;

    public PasswordEncripterBuilder()
    {
        _encripter = new Mock<IPasswordEncripter>();

        _encripter.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("string_criptography");
    }

    public PasswordEncripterBuilder Verify(string password)
    {
        if(string.IsNullOrWhiteSpace(password) == false)
        {
            _encripter.Setup(passwordEncripter => passwordEncripter.Verify(password, It.IsAny<string>())).Returns(true);
        }

        return this;
    }
    public IPasswordEncripter Build()
    {
        return _encripter.Object;
    }
}
