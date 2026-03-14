using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Atendimentos;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel.ReportExcelServices;
using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;

namespace UseCases.Tests.Atendimentos.Reports.Excel.ReportExcelServices;

public class ReportExcelServicesUseCaseTests
{
    private ReportExcelServicesUseCase CreateUseCase(User user, List<Atendimento> atendimentos)
    {
        var readOnlyRepository = new AtendimentosReadOnlyRepositoryBuilder().FilterServicesByMonth(user, atendimentos).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new ReportExcelServicesUseCase(readOnlyRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var atendimentos = AtendimentoBuilder.Collection(loggedUser);
        var useCase = CreateUseCase(loggedUser, atendimentos);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var atendimentos = AtendimentoBuilder.Collection(loggedUser);
        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.ShouldBeEmpty();
    }
}
