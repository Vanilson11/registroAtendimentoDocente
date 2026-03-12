using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Users;

public interface IUpdateOnlyUsersRepository
{
    Task<User?> GetById(long id);
    void Update(User user);
}
