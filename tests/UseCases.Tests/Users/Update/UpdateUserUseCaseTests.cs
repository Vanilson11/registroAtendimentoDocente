using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Application.UseCases.Users.Update;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Users.Update;

public class UpdateUserUseCaseTests
{
    private UpdateUserUseCase CreateUseCase(User? user = null, string? email = null)
    {
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().GetById(user).Build();
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().ExistActiveUserWithEmail(email).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new UpdateUserUseCase(updateOnlyRepository, readOnlyRepository, unitOffWork);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request, user.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request, id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.USER_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request, user.Id);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.NAME_USER_EMPTY));
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(request, user.Id);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }
}
