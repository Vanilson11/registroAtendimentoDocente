using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Users.DeleteProfile;
using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;

namespace UseCases.Tests.Users.DeleteProfile;

public class DeleteProfileUserUseCaseTests
{
    private DeleteProfileUserUseCase CreateUseCase(User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var writeOnlyRepository = WriteOnlyUsersRepositoryBuilder.Build();
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().GetById(user).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new DeleteProfileUserUseCase(loggedUser, writeOnlyRepository, updateOnlyRepository, unitOffWork);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute();

        await act.ShouldNotThrowAsync();
    }
}
