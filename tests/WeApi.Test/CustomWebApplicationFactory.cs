using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Enums;
using RegistroAtendimentoDocente.Domain.Security.Criptography;
using RegistroAtendimentoDocente.Domain.Security.Tokens;
using RegistroAtendimentoDocente.Infrastructure.DataAccess;
using WeApi.Test.Resources;

namespace WeApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public AtendimentoIdentityManager Atendimento_Others { get; private set; } = default!;
    public AtendimentoIdentityManager Atendimento_Admin { get; private set; } = default!;
    public AtendimentoIdentityManager Atendimento_Coordenador { get; private set; } = default!;
    public UserIdentityManager User_Others { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
    public UserIdentityManager User_Coordenador { get; private set; } = default!;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var serviceProvider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<RegistroAtendimentoDocenteDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(serviceProvider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<RegistroAtendimentoDocenteDbContext>();
                var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncrypter, accessTokenGenerator);
            });
    }

    private void StartDatabase(RegistroAtendimentoDocenteDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = AddUser(context, passwordEncripter, tokenGenerator);
        var atendimentoUser = AddAtendimento(user, context, atendimentoId: 1);
        Atendimento_Others = new AtendimentoIdentityManager(atendimentoUser);

        var userCoordenador = AddUserCoordenador(context, passwordEncripter, tokenGenerator);
        var atendimentoCoordenador = AddAtendimento(user, context, atendimentoId: 2);
        Atendimento_Coordenador = new AtendimentoIdentityManager(atendimentoCoordenador);

        var userAdmin = AddUserAdmin(context, passwordEncripter, tokenGenerator);
        var atendimentoAdmin = AddAtendimento(user, context, atendimentoId: 3);
        Atendimento_Admin = new AtendimentoIdentityManager(atendimentoAdmin);

        context.SaveChanges();
    }

    private User AddUser(RegistroAtendimentoDocenteDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Roles.OUTROS);
        user.Id = 1;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Others = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserCoordenador(RegistroAtendimentoDocenteDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Roles.COORDENADOR);
        user.Id = 2;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Coordenador = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(RegistroAtendimentoDocenteDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 3;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Atendimento AddAtendimento(User user, RegistroAtendimentoDocenteDbContext context, long atendimentoId)
    {
        var atendimento = AtendimentoBuilder.Build(user);
        atendimento.Id = atendimentoId;

        context.Atendimentos.Add(atendimento);

        return atendimento;
    }
}
