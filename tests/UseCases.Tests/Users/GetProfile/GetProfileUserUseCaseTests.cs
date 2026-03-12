using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetProfile;
using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;

namespace UseCases.Tests.Users.GetProfile;

public class GetProfileUserUseCaseTests
{
    private GetProfileUserUseCase CreateUseCase(User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().GetById(user).Build();
        var mapper = MapperBuilder.Build();

        return new GetProfileUserUseCase(loggedUser, readOnlyRepository, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(loggedUser.Name);
        result.Email.ShouldBe(loggedUser.Email);
        result.Role.ShouldBe(loggedUser.Role);
    }
}
