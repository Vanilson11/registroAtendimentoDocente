using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Atendimentos;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Delete;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Atendimentos.Delete;

public class DeleteAtendimentosUseCaseTests
{
    private DeleteAtendimentosUseCase CreateUseCase(User user, Atendimento? atendimento = null)
    {
        var writeOnlyRepository = AtendimentoWriteOnlyRepositoryBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var readOnlyRepository = new AtendimentosReadOnlyRepositoryBuilder().GetById(user, atendimento).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteAtendimentosUseCase(writeOnlyRepository, unitOffWork, readOnlyRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var atendimento = AtendimentoBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, atendimento);

        var act = async () => await useCase.Execute(atendimento.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Atendimento_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.SERVICE_NOT_FOUND));
    }
}
