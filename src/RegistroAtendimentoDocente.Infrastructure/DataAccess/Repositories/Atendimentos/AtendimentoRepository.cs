using Microsoft.EntityFrameworkCore;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories.Atendimentos;
internal class AtendimentoRepository : IAtendimentosReadOnlyRepository, IAtendimentoWriteOnlyRepository, IAtendimentoUpdateOnlyUseCase
{
    private readonly RegistroAtendimentoDocenteDbContext _dbContext;
    public AtendimentoRepository(RegistroAtendimentoDocenteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Atendimento>> GetAll(User user) 
    {
        return await _dbContext.Atendimentos.AsNoTracking().Where(u => u.UserId == user.Id).ToListAsync();
    }


    async Task<Atendimento?> IAtendimentosReadOnlyRepository.GetById(User user, long id) 
    {
        return await _dbContext.Atendimentos.AsNoTracking().FirstOrDefaultAsync(aten => aten.Id == id && aten.UserId == user.Id);
    }

    async Task<Atendimento?> IAtendimentoUpdateOnlyUseCase.GetById(User user, long id) 
    {
        return await _dbContext.Atendimentos.FirstOrDefaultAsync(aten => aten.Id == id && aten.UserId == user.Id);
    } 

    public async Task Add(Atendimento atendimento)
    {
        await _dbContext.AddAsync(atendimento);
    }
    public void Update(Atendimento atendimento)
    {
        _dbContext.Update(atendimento);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Atendimentos.FirstAsync(atend => atend.Id == id);

        _dbContext.Atendimentos.Remove(result);
    }

    public async Task<List<Atendimento>> FilterServicesByMonth(User user, DateOnly month)
    {
        var startMonth = new DateTime(year: month.Year, month.Month, day: 1).Date;
        var daysInMonth = DateTime.DaysInMonth(year: month.Year, month.Month);
        var endMonth = new DateTime(year: month.Year, month.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext.Atendimentos.AsNoTracking()
            .Where(service => service.Data >= startMonth && service.Data <= endMonth && service.UserId == user.Id)
            .OrderBy(service => service.Data)
            .ThenBy(service => service.Docente)
            .ToListAsync();
    }
}
