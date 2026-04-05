using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Consultations;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportExcelConsultations;
using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;

namespace UseCases.Tests.Consultations.Reports.Excel.ReportExcelConsultations;

public class ReportExcelConsultationsUseCaseTests
{
    private ReportExcelConsultationUseCase CreateUseCase(User user, List<Consultation> consultations)
    {
        var readOnlyRepository = new ConsultationsReadOnlyRepositoryBuilder().FilterServicesByMonth(user, consultations).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new ReportExcelConsultationUseCase(readOnlyRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var consultations = ConsultationBuilder.Collection(loggedUser);
        var useCase = CreateUseCase(loggedUser, consultations);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.ShouldBeEmpty();
    }
}
