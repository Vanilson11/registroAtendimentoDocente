using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Users;
public interface IReadOnlyUsersRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<User?> GetByEmail(string email);
}
