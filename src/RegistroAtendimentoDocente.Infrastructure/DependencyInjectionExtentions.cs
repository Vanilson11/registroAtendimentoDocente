using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
using RegistroAtendimentoDocente.Infrastructure.DataAccess;
using RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories;

namespace RegistroAtendimentoDocente.Infrastructure;
public static class DependencyInjectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOffWork, UnitOffWork>();
        services.AddScoped<IAtendimentosReadOnlyRepository, AtendimentoRepository>();
        services.AddScoped<IAtendimentoWriteOnlyRepository, AtendimentoRepository>();
        services.AddScoped<IAtendimentoUpdateOnlyUseCase, AtendimentoRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

        services.AddDbContext<RegistroAtendimentoDocenteDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}
