using Microsoft.EntityFrameworkCore;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories;
internal class AtendimentoRepository : IAtendimentosReadOnlyRepository, IAtendimentoWriteOnlyRepository, IAtendimentoUpdateOnlyUseCase
{
    private readonly RegistroAtendimentoDocenteDbContext _dbContext;
    public AtendimentoRepository(RegistroAtendimentoDocenteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Atendimento>> GetAll() => await _dbContext.Atendimentos.AsNoTracking().ToListAsync();

    async Task<Atendimento?> IAtendimentosReadOnlyRepository.GetById(int id) => await _dbContext.Atendimentos.AsNoTracking().FirstOrDefaultAsync(aten => aten.Id == id);

    async Task<Atendimento?> IAtendimentoUpdateOnlyUseCase.GetById(int id) => await _dbContext.Atendimentos.FirstOrDefaultAsync(aten => aten.Id == id);

    public async Task Add(Atendimento atendimento)
    {
        await _dbContext.AddAsync(atendimento);
    }
    public void Update(Atendimento atendimento)
    {
        _dbContext.Update(atendimento);
    }

    public async Task<bool> Delete(int id)
    {
        var result = await _dbContext.Atendimentos.FirstOrDefaultAsync(atend => atend.Id == id);

        if (result is null) return false;

        _dbContext.Atendimentos.Remove(result);

        return true;
    }

    public async Task<List<Atendimento>> FilterServicesByMonth(DateOnly month)
    {
        var startMonth = new DateTime(year: month.Year, month.Month, day: 1).Date;
        var daysInMonth = DateTime.DaysInMonth(year: month.Year, month.Month);
        var endMonth = new DateTime(year: month.Year, month.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext.Atendimentos.AsNoTracking()
            .Where(service => service.Data >= startMonth && service.Data <= endMonth)
            .OrderBy(service => service.Data)
            .ThenBy(service => service.Docente)
            .ToListAsync();
    }
}
