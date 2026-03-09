using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Users;
public interface IWriteOnlyUsersRepository
{
    Task Add(User user);
}
