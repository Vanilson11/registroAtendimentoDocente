using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;

namespace CommonTestUtilities.Repositories.Atendimentos;

public class AtendimentosReadOnlyRepositoryBuilder
{
    private readonly Mock<IAtendimentosReadOnlyRepository> _mock;

    public AtendimentosReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IAtendimentosReadOnlyRepository>();
    }

    public AtendimentosReadOnlyRepositoryBuilder GetAll(User user, List<Atendimento> atendimentos)
    {
        _mock.Setup(repository => repository.GetAll(user)).ReturnsAsync(atendimentos);

        return this;
    }

    public AtendimentosReadOnlyRepositoryBuilder GetById(User user, Atendimento? atendimento)
    {
        if(atendimento is not null)
        {
            _mock.Setup(repository => repository.GetById(user, It.IsAny<long>())).ReturnsAsync(atendimento);
        }

        return this;
    }

    public AtendimentosReadOnlyRepositoryBuilder FilterServicesByMonth(User user, List<Atendimento> atendimentos)
    {
        _mock.Setup(repository => repository.FilterServicesByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(atendimentos);

        return this;
    }

    public IAtendimentosReadOnlyRepository Build() { return _mock.Object; }
}
