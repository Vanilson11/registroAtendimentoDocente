namespace RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;
public interface IReadOnlyUsersRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
}
