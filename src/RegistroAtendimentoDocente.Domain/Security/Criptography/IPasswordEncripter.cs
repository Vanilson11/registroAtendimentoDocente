namespace RegistroAtendimentoDocente.Domain.Security.Criptography;
public interface IPasswordEncripter
{
    string Encrypt(string password);
    bool Verify(string password, string hashPassword);
}
