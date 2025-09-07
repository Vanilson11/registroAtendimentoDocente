using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Infrastructure.DataAccess;

namespace RegistroAtendimentoDocente.Infrastructure.Migrations;
public static class DataBaseMigration
{
    public static async Task MigrateDataBase(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<RegistroAtendimentoDocenteDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
