using RegistroAtendimentoDocente.Domain.Repositories;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess;
internal class UnitOffWork : IUnitOffWork
{
    private readonly RegistroAtendimentoDocenteDbContext _dbContext;
    public UnitOffWork(RegistroAtendimentoDocenteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
