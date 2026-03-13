using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Atendimentos;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel.ReportByCoordenador;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Enums;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Atendimentos.Reports.Excel.ReportByCoordenador;

public class ReportExcelByCoordenadorUseCaseTests
{
    private ReportExcelByCoordenadorUseCase CreateUseCase(
        List<Atendimento> atendimentos, User userLogged, User? user = null
        )
    {
        var readOnlyAtendimentoRepository = new AtendimentosReadOnlyRepositoryBuilder().GetAll(user, atendimentos).Build();
        var readOnlyUsersRepository = new ReadOnlyUsersRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(userLogged);

        return new ReportExcelByCoordenadorUseCase(readOnlyAtendimentoRepository, readOnlyUsersRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build(Roles.ADMIN);
        var user = UserBuilder.Build();
        var atendimentos = AtendimentoBuilder.Collection(user);
        var useCase = CreateUseCase(atendimentos, loggedUser, user);

        var result = await useCase.Execute(user.Id);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build(Roles.ADMIN);
        var user = UserBuilder.Build();
        var useCase = CreateUseCase([], loggedUser, user);

        var result = await useCase.Execute(user.Id);

        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var loggedUser = UserBuilder.Build(Roles.ADMIN);
        var useCase = CreateUseCase([], loggedUser);

        var act = async () => await useCase.Execute(id: 100);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.USER_NOT_FOUND));
    }

    [Fact]
    public async Task Error_User_Not_Coordinator()
    {
        var loggedUser = UserBuilder.Build(Roles.ADMIN);
        var user = UserBuilder.Build(Roles.OUTROS);
        user.Id = 2;
        var useCase = CreateUseCase([], loggedUser, user);

        var act = async () => await useCase.Execute(user.Id);

        var result = await act.ShouldThrowAsync<UserNotCoordinatorException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.USER_IS_NOT_COORDINATOR));
    }
}
