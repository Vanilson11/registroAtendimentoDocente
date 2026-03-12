using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Atendimentos;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetAll;
using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;

namespace UseCases.Tests.Atendimentos.GetAll;

public class GetAllAtendimentosUseCaseTests
{
    private GetAllAtendimentosUseCase CreateUseCase(User user, List<Atendimento> atendimentos)
    {
        var readOnlyRepository = new AtendimentosReadOnlyRepositoryBuilder().GetAll(user, atendimentos).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllAtendimentosUseCase(readOnlyRepository, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var atendimentos = AtendimentoBuilder.Collection(loggedUser);
        var useCase = CreateUseCase(loggedUser, atendimentos);

        var result = await useCase.Execute();

        result.Atendimentos.ShouldNotBeNull();
        result.Atendimentos.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.Execute();

        result.Atendimentos.ShouldBeEmpty();
    }
}
