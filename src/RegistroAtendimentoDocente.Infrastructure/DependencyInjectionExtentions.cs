using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
using RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;
using RegistroAtendimentoDocente.Domain.Security.Criptography;
using RegistroAtendimentoDocente.Domain.Security.Tokens;
using RegistroAtendimentoDocente.Infrastructure.DataAccess;
using RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories;
using RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories.UsersRepositoy;
using RegistroAtendimentoDocente.Infrastructure.Security.Tokens;

namespace RegistroAtendimentoDocente.Infrastructure;
public static class DependencyInjectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
        AddTokens(services, configuration);

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

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
        var expireMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinues");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expireMinutes, signingKey!));
    }
}
