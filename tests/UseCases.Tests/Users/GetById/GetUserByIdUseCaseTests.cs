using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Users;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetById;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Users.GetById;

public class GetUserByIdUseCaseTests
{
    private GetUserByIdUseCase CreateUseCase(User? user = null)
    {
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().GetById(user).Build();
        var mapper = MapperBuilder.Build();

        return new GetUserByIdUseCase(readOnlyRepository, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(user.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(user.Id);
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);
        result.Role.ShouldBe(user.Role);
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.USER_NOT_FOUND));
    }
}
