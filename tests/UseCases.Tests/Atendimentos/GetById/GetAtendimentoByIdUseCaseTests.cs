using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Atendimentos;
using CommonTestUtilities.Services.LoggedUser;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetById;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Atendimentos.GetById;

public class GetAtendimentoByIdUseCaseTests
{
    private GetAtendimentoByIdUseCase CreateUseCase(User user, Atendimento? atendimento = null)
    {
        var readOnlyRepository = new AtendimentosReadOnlyRepositoryBuilder().GetById(user, atendimento).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAtendimentoByIdUseCase(readOnlyRepository, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var atendimento = AtendimentoBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, atendimento);

        var result = await useCase.Execute(atendimento.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(atendimento.Id);
        result.Docente.ShouldBe(atendimento.Docente);
        result.Assunto.ShouldBe(atendimento.Assunto);
        result.Data.ShouldBe(atendimento.Data);
        result.Encaminhamento.ShouldBe(atendimento.Encaminhamento);
        result.Observacao.ShouldBe(atendimento.Observacao);
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
