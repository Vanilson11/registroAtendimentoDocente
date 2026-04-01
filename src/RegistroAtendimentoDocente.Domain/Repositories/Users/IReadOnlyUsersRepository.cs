using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.Users;
public interface IReadOnlyUsersRepository
{
    Task<List<User>> GetAll();
    Task<List<User>> GetAllCoordinators();
    Task<User?> GetById(long id);
    Task<User?> GetByEmail(string email);
    Task<bool> ExistActiveUserWithEmail(string email);
}
