using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    Task<User> Get();
}
