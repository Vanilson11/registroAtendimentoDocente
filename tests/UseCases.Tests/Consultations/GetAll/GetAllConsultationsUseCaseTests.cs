using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Consultations;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.GetAll;
using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;

namespace UseCases.Tests.Consultations.GetAll;

public class GetAllConsultationsUseCaseTests
{
    private GetAllConsultationsUseCase CreateUseCase(User user, List<Consultation> consultations)
    {
        var readOnlyRepository = new ConsultationsReadOnlyRepositoryBuilder().GetAll(user, consultations).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllConsultationsUseCase(readOnlyRepository, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var consultations = ConsultationBuilder.Collection(loggedUser);
        var useCase = CreateUseCase(loggedUser, consultations);

        var result = await useCase.Execute();

        result.Consultation.ShouldNotBeNull();
        result.Consultation.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.Execute();

        result.Consultation.ShouldBeEmpty();
    }
}
