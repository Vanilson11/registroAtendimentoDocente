using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using DocumentFormat.OpenXml.Spreadsheet;
using RegistroAtendimentoDocente.Application.UseCases.Users.Delete;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Users.Delete;
public class DeleteUserUseCaseTests
{
    private DeleteUserUseCase CreateUseCase(User? user = null)
    {
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().GetById(user).Build();
        var writeOnlyRepository = WriteOnlyUsersRepositoryBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new DeleteUserUseCase(updateOnlyRepository, writeOnlyRepository, unitOffWork);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(user.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id: 100);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.USER_NOT_FOUND));
    }
}
