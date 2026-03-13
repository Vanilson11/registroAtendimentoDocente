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
    public AtendimentoIdentityManager Atendimento_Admin { get; private set; } = default!;
    public AtendimentoIdentityManager Atendimento_Coordenador { get; private set; } = default!;
    public UserIdentityManager User_Others { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
    public UserIdentityManager User_Coordenador_1 { get; private set; } = default!;
    public UserIdentityManager User_Coordenador_2 { get; private set; } = default!;
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
        AddUser(context, passwordEncripter, tokenGenerator, id: 1);

        var userCoordenador1 = AddUserCoordenador(context, passwordEncripter, tokenGenerator, id: 2);
        User_Coordenador_1 = userCoordenador1;

        var atendimentoCoordenador1 = AddAtendimento(userCoordenador1.GetUser(), context, atendimentoId: 1);
        Atendimento_Coordenador = new AtendimentoIdentityManager(atendimentoCoordenador1);

        var userCoordenador2 = AddUserCoordenador(context, passwordEncripter, tokenGenerator, id: 3);
        User_Coordenador_2 = userCoordenador2;

        var userAdmin = AddUserAdmin(context, passwordEncripter, tokenGenerator, id: 4);

        var atendimentoAdmin = AddAtendimento(userAdmin, context, atendimentoId: 2);
        Atendimento_Admin = new AtendimentoIdentityManager(atendimentoAdmin);

        context.SaveChanges();
    }

    private void AddUser(
        RegistroAtendimentoDocenteDbContext context, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator,
        long id)
    {
        var user = UserBuilder.Build(Roles.OUTROS);
        user.Id = id;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Others = new UserIdentityManager(user, password, token);
    }

    private UserIdentityManager AddUserCoordenador(
        RegistroAtendimentoDocenteDbContext context, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator,
        long id)
    {
        var user = UserBuilder.Build(Roles.COORDENADOR);
        user.Id = id;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        return new UserIdentityManager(user, password, token);
    }

    private User AddUserAdmin(
        RegistroAtendimentoDocenteDbContext context, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator,
        long id)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id =id;

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
