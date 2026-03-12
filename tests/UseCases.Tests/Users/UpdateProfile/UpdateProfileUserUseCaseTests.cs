using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Users.UpdateProfile;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Users.UpdateProfile;

public class UpdateProfileUserUseCaseTests
{
    private UpdateProfileUserUseCase CreateUseCase(User user, string? email = null) 
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().GetById(user).Build();
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().ExistActiveUserWithEmail(email).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new UpdateProfileUserUseCase(loggedUser, updateOnlyRepository, readOnlyRepository, unitOffWork);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        loggedUser.Name.ShouldBe(request.Name);
        loggedUser.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Error_Name_User_Empty()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = string.Empty;
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.NAME_USER_EMPTY));
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }
}
