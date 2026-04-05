using Microsoft.EntityFrameworkCore;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories.Consultations;
internal class ConsultationsRepository : IConsultationsReadOnlyRepository, IConsultationsWriteOnlyRepository, IConsultationsUpdateOnlyUseCase
{
    private readonly RegisterConsultationsTeacherDbContext _dbContext;
    public ConsultationsRepository(RegisterConsultationsTeacherDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Consultation>> GetAll(User user) 
    {
        return await _dbContext.Consultations.AsNoTracking().Where(u => u.UserId == user.Id).ToListAsync();
    }


    async Task<Consultation?> IConsultationsReadOnlyRepository.GetById(User user, long id) 
    {
        return await _dbContext.Consultations.AsNoTracking().FirstOrDefaultAsync(consultation => consultation.Id == id && consultation.UserId == user.Id);
    }

    async Task<Consultation?> IConsultationsUpdateOnlyUseCase.GetById(User user, long id) 
    {
        return await _dbContext.Consultations.FirstOrDefaultAsync(consultation => consultation.Id == id && consultation.UserId == user.Id);
    } 

    public async Task Add(Consultation consultation)
    {
        await _dbContext.AddAsync(consultation);
    }
    public void Update(Consultation consultation)
    {
        _dbContext.Update(consultation);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Consultations.FirstAsync(consultation => consultation.Id == id);

        _dbContext.Consultations.Remove(result);
    }

    public async Task<List<Consultation>> FilterConsultationsByMonth(User user, DateOnly month)
    {
        var startMonth = new DateTime(year: month.Year, month.Month, day: 1).Date;
        var daysInMonth = DateTime.DaysInMonth(year: month.Year, month.Month);
        var endMonth = new DateTime(year: month.Year, month.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext.Consultations.AsNoTracking()
            .Where(consultation => consultation.Date >= startMonth && consultation.Date <= endMonth && consultation.UserId == user.Id)
            .OrderBy(consultation => consultation.Date)
            .ThenBy(consultation => consultation.Teacher)
            .ToListAsync();
    }
}
