using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Consultations;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Register;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Consultations.Register;

public class RegisterConsultationUseCaseTests
{
    private RegisterConsultationUseCase CreateUseCase(User user)
    {
        var writeOnlyRepository = ConsultationsWriteOnlyRepositoryBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterConsultationUseCase(writeOnlyRepository, unitOffWork, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestConsultationJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThanOrEqualTo(0);
        result.Teacher.ShouldBe(request.Teacher);
        result.Subject.ShouldBe(request.Subject);
    }

    [Fact]
    public async Task Error_Name_Teacher_Empty()
    {
        var request = RequestConsultationJsonBuilder.Build();
        request.Teacher = string.Empty;
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.TEACHER_NAME_EMPTY));
    }
}
