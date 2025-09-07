using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;
public interface IWriteOnlyUsersRepository
{
    Task Add(User user);
}
