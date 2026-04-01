using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Users;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetAllCoordinators;
using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;

namespace UseCases.Tests.Users.GetAllCoordinators;

public class GetAllCoordinatorsUseCaseTests
{
    private GetAllCoordinatorsUseCase CreateUseCase(List<User> coordinators)
    {
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().GetAllCoordinators(coordinators).Build();
        var mapper = MapperBuilder.Build();

        return new GetAllCoordinatorsUseCase(readOnlyRepository, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var coordinators = UserBuilder.Collection();
        var useCase = CreateUseCase(coordinators);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Users.ShouldNotBeNull();
        result.Users.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var coordinators = UserBuilder.Collection();
        var useCase = CreateUseCase([]);

        var result = await useCase.Execute();

        result.Users.ShouldBeEmpty();
    }
}
