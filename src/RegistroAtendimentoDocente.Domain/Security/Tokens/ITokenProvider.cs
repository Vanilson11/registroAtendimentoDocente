namespace RegistroAtendimentoDocente.Domain.Security.Tokens;
public interface ITokenProvider
{
    string GetTokenOnRequest();
}
