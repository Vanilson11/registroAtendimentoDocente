using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Atendimentos;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Update;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Atendimentos.Update;

public class UpdateAtendimentoUseCaseTests
{
    private UpdateAtendimentoUseCase CreateUseCase(User user, Atendimento? atendimento = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var updateOnlyRepository = new AtendimentoUpdateOnlyUseCaseBuilder().GetById(user, atendimento).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateAtendimentoUseCase(mapper, unitOffWork, updateOnlyRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestAtendimentoJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var atendimento = AtendimentoBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, atendimento);

        var act = async () => await useCase.Execute(request, atendimento.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Name_Docente_Empty()
    {
        var request = RequestAtendimentoJsonBuilder.Build();
        request.Docente = string.Empty;
        var loggedUser = UserBuilder.Build();
        var atendimento = AtendimentoBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, atendimento);

        var act = async () => await useCase.Execute(request, atendimento.Id);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.TEACHER_NAME_EMPTY));
    }

    [Fact]
    public async Task Error_Atendimento_Not_Found()
    {
        var request = RequestAtendimentoJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request, id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.SERVICE_NOT_FOUND));
    }
}
