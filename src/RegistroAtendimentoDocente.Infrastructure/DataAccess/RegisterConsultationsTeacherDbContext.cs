using Microsoft.EntityFrameworkCore;
using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess;
public class RegisterConsultationsTeacherDbContext : DbContext
{
    public RegisterConsultationsTeacherDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<User> Users { get; set; }
}
