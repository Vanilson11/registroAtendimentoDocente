using RegistroAtendimentoDocente.Domain.Security.Criptography;
using BC = BCrypt.Net.BCrypt;

namespace RegistroAtendimentoDocente.Infrastructure.Security.Criptography;
public class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        var passwordHash = BC.HashPassword(password);

        return passwordHash;
    }
}
