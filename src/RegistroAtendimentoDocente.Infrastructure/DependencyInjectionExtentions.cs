using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
using RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;
using RegistroAtendimentoDocente.Domain.Security.Criptography;
using RegistroAtendimentoDocente.Infrastructure.DataAccess;
using RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories;
using RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories.UsersRepositoy;

namespace RegistroAtendimentoDocente.Infrastructure;
public static class DependencyInjectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);

        services.AddScoped<IPasswordEncripter, Security.Criptography.BCrypt>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOffWork, UnitOffWork>();
        services.AddScoped<IAtendimentosReadOnlyRepository, AtendimentoRepository>();
        services.AddScoped<IAtendimentoWriteOnlyRepository, AtendimentoRepository>();
        services.AddScoped<IAtendimentoUpdateOnlyUseCase, AtendimentoRepository>();
        services.AddScoped<IReadOnlyUsersRepository, UsersRepository>();
        services.AddScoped<IWriteOnlyUsersRepository, UsersRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

        services.AddDbContext<RegistroAtendimentoDocenteDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}
