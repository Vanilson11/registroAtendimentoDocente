using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Security.Criptography;
using RegistroAtendimentoDocente.Infrastructure.DataAccess;

namespace WeApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user;
    private string _password;
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

                StartDatabase(dbContext, passwordEncrypter);
            });
    }

    public string GetName() => _user.Name;
    public string GetPassword() => _password;
    public string GetEmail() => _user.Email;

    private void StartDatabase(RegistroAtendimentoDocenteDbContext context, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();

        _password = _user.Password;

        _user.Password = passwordEncripter.Encrypt(_user.Password);

        context.Users.Add(_user);

        context.SaveChanges();
    }
}
