using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;

namespace CommonTestUtilities.Repositories.Atendimentos;

public class AtendimentoUpdateOnlyUseCaseBuilder
{
    private readonly Mock<IAtendimentoUpdateOnlyUseCase> _mock;

    public AtendimentoUpdateOnlyUseCaseBuilder()
    {
        _mock = new Mock<IAtendimentoUpdateOnlyUseCase>();
    }

    public AtendimentoUpdateOnlyUseCaseBuilder GetById(User user, Atendimento? atendimento)
    {
        if(atendimento is not null)
        {
            _mock.Setup(repository => repository.GetById(user, It.IsAny<long>())).ReturnsAsync(atendimento);
        }
        
        return this;
    }

    public IAtendimentoUpdateOnlyUseCase Build() { return _mock.Object; }
}
