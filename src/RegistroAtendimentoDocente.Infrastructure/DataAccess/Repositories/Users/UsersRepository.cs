using Microsoft.EntityFrameworkCore;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Users;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories.Users;
internal class UsersRepository : IReadOnlyUsersRepository, IWriteOnlyUsersRepository, IUpdateOnlyUsersRepository
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

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    async Task<User?> IReadOnlyUsersRepository.GetById(long id)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id.Equals(id));
    }

    async Task<User?> IUpdateOnlyUsersRepository.GetById(long id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }

    public async Task Delete(long id)
    {
        var user = await _dbContext.Users.FirstAsync(u => u.Id.Equals(id));

        _dbContext.Users.Remove(user);
    }

    public async Task<List<User>> GetAll()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }
}
