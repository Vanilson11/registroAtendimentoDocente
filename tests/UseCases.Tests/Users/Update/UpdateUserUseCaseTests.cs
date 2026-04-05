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
    private UpdateUserUseCase CreateUseCase(User? user = null)
    {
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().GetById(user).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new UpdateUserUseCase(updateOnlyRepository, unitOffWork);
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
    public async Task Error_Role_Empty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Role = string.Empty;

        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request, user.Id);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.ROLE_EMPTY));
    }

    [Fact]
    public async Task Error_Role_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build(role: "invalidRole");

        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request, user.Id);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.ROLE_INVALID));
    }
}
