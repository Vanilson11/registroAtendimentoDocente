using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Security.Tokens;
public interface IAccessTokenGenerator
{
    string Genetate(User user);
}
