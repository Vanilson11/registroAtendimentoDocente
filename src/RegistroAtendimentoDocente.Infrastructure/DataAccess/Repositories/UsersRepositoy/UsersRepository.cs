using Microsoft.EntityFrameworkCore;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories.UsersRepositoy;
internal class UsersRepository : IReadOnlyUsersRepository, IWriteOnlyUsersRepository
{
    private readonly RegistroAtendimentoDocenteDbContext _dbContext;

    public UsersRepository(RegistroAtendimentoDocenteDbContext dbContext)
    {
       _dbContext = dbContext; 
    }

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }
}
