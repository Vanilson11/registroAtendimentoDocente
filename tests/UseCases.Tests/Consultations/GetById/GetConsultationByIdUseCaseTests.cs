using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Consultations;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.GetById;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Consultations.GetById;

public class GetConsultationByIdUseCaseTests
{
    private GetConsultationByIdUseCase CreateUseCase(User user, Consultation? consultation = null)
    {
        var readOnlyRepository = new ConsultationsReadOnlyRepositoryBuilder().GetById(user, consultation).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetConsultationByIdUseCase(readOnlyRepository, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var consultation = ConsultationBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, consultation);

        var result = await useCase.Execute(consultation.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(consultation.Id);
        result.Teacher.ShouldBe(consultation.Teacher);
        result.Subject.ShouldBe(consultation.Subject);
        result.Date.ShouldBe(consultation.Date);
        result.Recommendations.ShouldBe(consultation.Recommendations);
        result.Observation.ShouldBe(consultation.Observation);
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
