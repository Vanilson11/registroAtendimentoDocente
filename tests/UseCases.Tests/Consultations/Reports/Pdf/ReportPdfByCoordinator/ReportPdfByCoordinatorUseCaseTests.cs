using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Consultations;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Pdf.ReportPdfByCoordinator;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Enums;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Consultations.Reports.Pdf.ReportPdfByCoordinator;

public class ReportPdfByCoordinatorUseCaseTests
{
    private ReportPdfByCoordinatorUseCase CreateUseCase(
        List<Consultation> consultations, User userLogged, User? user = null
        )
    {
        var readOnlyConsultationRepository = new ConsultationsReadOnlyRepositoryBuilder().GetAll(user, consultations).Build();
        var readOnlyUsersRepository = new ReadOnlyUsersRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(userLogged);

        return new ReportPdfByCoordinatorUseCase(loggedUser, readOnlyConsultationRepository, readOnlyUsersRepository);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build(Roles.ADMIN);
        var user = UserBuilder.Build();
        var consultations = ConsultationBuilder.Collection(user);
        var useCase = CreateUseCase(consultations, loggedUser, user);

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
        var user = UserBuilder.Build(Roles.OTHERS);
        user.Id = 2;
        var useCase = CreateUseCase([], loggedUser, user);

        var act = async () => await useCase.Execute(user.Id);

        var result = await act.ShouldThrowAsync<UserNotCoordinatorException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.USER_IS_NOT_COORDINATOR));
    }
}
