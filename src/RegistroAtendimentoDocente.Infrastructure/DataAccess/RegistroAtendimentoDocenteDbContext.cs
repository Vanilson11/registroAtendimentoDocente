using Microsoft.EntityFrameworkCore;
using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Infrastructure.DataAccess;
internal class RegistroAtendimentoDocenteDbContext : DbContext
{
    public RegistroAtendimentoDocenteDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Atendimento> Atendimentos { get; set; }
}
