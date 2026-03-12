using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptograph;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Users.ChangePassword;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Users.ChangePassword;

public class ChangePasswordUseCaseTests
{
    private ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().GetById(user).Build();
        var passwordEncrypter = new PasswordEncripterBuilder().Verify(password).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, updateOnlyRepository, passwordEncrypter, unitOffWork);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;
        var useCase = CreateUseCase(loggedUser, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.PASSWORD_EMPTY));
    }

    [Fact]
    public async Task Error_Password_Different_Current_Password()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
    }
}
