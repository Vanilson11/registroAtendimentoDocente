using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Consultations;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Delete;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Consultations.Delete;

public class DeleteConsultationsUseCaseTests
{
    private DeleteConsultationUseCase CreateUseCase(User user, Consultation? consultation = null)
    {
        var writeOnlyRepository = ConsultationsWriteOnlyRepositoryBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var readOnlyRepository = new ConsultationsReadOnlyRepositoryBuilder().GetById(user, consultation).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteConsultationUseCase(writeOnlyRepository, unitOffWork, readOnlyRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var consultation = ConsultationBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, consultation);

        var act = async () => await useCase.Execute(consultation.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Consultation_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.CONSULTATION_NOT_FOUND));
    }
}
