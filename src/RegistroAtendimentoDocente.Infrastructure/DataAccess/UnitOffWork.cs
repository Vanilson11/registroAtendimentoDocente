using RegistroAtendimentoDocente.Domain.Repositories;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess;
internal class UnitOffWork : IUnitOffWork
{
    private readonly RegisterConsultationsTeacherDbContext _dbContext;
    public UnitOffWork(RegisterConsultationsTeacherDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
