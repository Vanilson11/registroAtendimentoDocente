using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;
public interface IReadOnlyUsersRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<User?> GetByEmail(string email);
}
