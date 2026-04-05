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
    public ConsultationIdentityManager Consultation_Admin { get; private set; } = default!;
    public ConsultationIdentityManager Consultation_Coordinator { get; private set; } = default!;
    public UserIdentityManager User_Others { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
    public UserIdentityManager User_Coordinator_1 { get; private set; } = default!;
    public UserIdentityManager User_Coordinator_2 { get; private set; } = default!;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var serviceProvider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<RegisterConsultationsTeacherDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(serviceProvider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<RegisterConsultationsTeacherDbContext>();
                var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncrypter, accessTokenGenerator);
            });
    }

    private void StartDatabase(RegisterConsultationsTeacherDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        AddUser(context, passwordEncripter, tokenGenerator, id: 1);

        var userCoordinator1 = AddUserCoordinator(context, passwordEncripter, tokenGenerator, id: 2);
        User_Coordinator_1 = userCoordinator1;

        var consultationCoordinator1 = AddConsultation(userCoordinator1.GetUser(), context, consultationId: 1);
        Consultation_Coordinator = new ConsultationIdentityManager(consultationCoordinator1);

        var userCoordinator2 = AddUserCoordinator(context, passwordEncripter, tokenGenerator, id: 3);
        User_Coordinator_2 = userCoordinator2;

        var userAdmin = AddUserAdmin(context, passwordEncripter, tokenGenerator, id: 4);

        var consultationAdmin = AddConsultation(userAdmin, context, consultationId: 2);
        Consultation_Admin = new ConsultationIdentityManager(consultationAdmin);

        context.SaveChanges();
    }

    private void AddUser(
        RegisterConsultationsTeacherDbContext context, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator,
        long id)
    {
        var user = UserBuilder.Build(Roles.OTHERS);
        user.Id = id;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Others = new UserIdentityManager(user, password, token);
    }

    private UserIdentityManager AddUserCoordinator(
        RegisterConsultationsTeacherDbContext context, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator,
        long id)
    {
        var user = UserBuilder.Build(Roles.COORDINATOR);
        user.Id = id;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        return new UserIdentityManager(user, password, token);
    }

    private User AddUserAdmin(
        RegisterConsultationsTeacherDbContext context, 
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

    private Consultation AddConsultation(User user, RegisterConsultationsTeacherDbContext context, long consultationId)
    {
        var consultation = ConsultationBuilder.Build(user);
        consultation.Id = consultationId;

        context.Consultations.Add(consultation);

        return consultation;
    }
}
