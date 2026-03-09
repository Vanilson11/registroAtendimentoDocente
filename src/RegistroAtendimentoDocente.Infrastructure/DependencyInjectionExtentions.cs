using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Domain.Security.Criptography;
using RegistroAtendimentoDocente.Domain.Security.Tokens;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Infrastructure.DataAccess;
using RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories.Atendimentos;
using RegistroAtendimentoDocente.Infrastructure.DataAccess.Repositories.Users;
using RegistroAtendimentoDocente.Infrastructure.Security.Tokens;
using RegistroAtendimentoDocente.Infrastructure.Services.LoggedUser;

namespace RegistroAtendimentoDocente.Infrastructure;
public static class DependencyInjectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if(configuration.IsTestEnvironment() == false)
        {
            AddDbContext(services, configuration);
        }
        
        AddRepositories(services);
        AddTokens(services, configuration);

        services.AddScoped<IPasswordEncripter, Security.Criptography.BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();
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
