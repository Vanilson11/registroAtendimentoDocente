using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Users;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetAll;
using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;

namespace UseCases.Tests.Users.GetAll;

public class GetAllUsersUseCaseTests
{
    private GetAllUsersUseCase CreateUseCase(List<User> users)
    {
        var mapper = MapperBuilder.Build();
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().GetAll(users).Build();

        return new GetAllUsersUseCase(mapper, readOnlyRepository);
    }

    [Fact]
    public async Task Success()
    {
        var users = UserBuilder.Collection();
        var useCase = CreateUseCase(users);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Users.ShouldNotBeEmpty();
        result.Users.ForEach(user =>
        {
            user.Name.ShouldNotBeNullOrWhiteSpace();
            user.Email.ShouldNotBeNullOrWhiteSpace();
            user.Role.ShouldNotBeNullOrWhiteSpace();
        });
    }

    [Fact]
    public async Task Success_Empty()
    {
        var users = UserBuilder.Collection();
        var useCase = CreateUseCase([]);

        var result = await useCase.Execute();

        result.Users.ShouldBeEmpty();
    }
}
