using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Atendimentos;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Register;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Enums;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Atendimentos.Register;

public class RegisterAtendimentoUseCaseTests
{
    private RegisterAtendimentoUseCase CreateUseCase(User user)
    {
        var writeOnlyRepository = AtendimentoWriteOnlyRepositoryBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterAtendimentoUseCase(writeOnlyRepository, unitOffWork, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestAtendimentoJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThanOrEqualTo(0);
        result.Docente.ShouldBe(request.Docente);
        result.Assunto.ShouldBe(request.Assunto);
    }

    [Fact]
    public async Task Error_Name_Docente_Empty()
    {
        var request = RequestAtendimentoJsonBuilder.Build();
        request.Docente = string.Empty;
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.TEACHER_NAME_EMPTY));
    }
}
