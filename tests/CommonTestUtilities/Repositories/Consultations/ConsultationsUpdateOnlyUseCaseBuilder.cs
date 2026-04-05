using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;

namespace CommonTestUtilities.Repositories.Consultations;

public class ConsultationsUpdateOnlyUseCaseBuilder
{
    private readonly Mock<IConsultationsUpdateOnlyUseCase> _mock;

    public ConsultationsUpdateOnlyUseCaseBuilder()
    {
        _mock = new Mock<IConsultationsUpdateOnlyUseCase>();
    }

    public ConsultationsUpdateOnlyUseCaseBuilder GetById(User user, Consultation? consultation)
    {
        if(consultation is not null)
        {
            _mock.Setup(repository => repository.GetById(user, It.IsAny<long>())).ReturnsAsync(consultation);
        }
        
        return this;
    }

    public IConsultationsUpdateOnlyUseCase Build() { return _mock.Object; }
}
