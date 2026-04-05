using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Consultations;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Update;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Consultations.Update;

public class UpdateConsultationUseCaseTests
{
    private UpdateAtendimentoUseCase CreateUseCase(User user, Consultation? consultation = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var updateOnlyRepository = new ConsultationsUpdateOnlyUseCaseBuilder().GetById(user, consultation).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateAtendimentoUseCase(mapper, unitOffWork, updateOnlyRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestConsultationJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var consultation = ConsultationBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, consultation);

        var act = async () => await useCase.Execute(request, consultation.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Name_Teacher_Empty()
    {
        var request = RequestConsultationJsonBuilder.Build();
        request.Teacher = string.Empty;
        var loggedUser = UserBuilder.Build();
        var consultation = ConsultationBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, consultation);

        var act = async () => await useCase.Execute(request, consultation.Id);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.TEACHER_NAME_EMPTY));
    }

    [Fact]
    public async Task Error_Consultation_Not_Found()
    {
        var request = RequestConsultationJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request, id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.CONSULTATION_NOT_FOUND));
    }
}
