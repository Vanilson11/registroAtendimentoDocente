using Moq;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;

namespace CommonTestUtilities.Repositories.Consultations;

public class ConsultationsReadOnlyRepositoryBuilder
{
    private readonly Mock<IConsultationsReadOnlyRepository> _mock;

    public ConsultationsReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IConsultationsReadOnlyRepository>();
    }

    public ConsultationsReadOnlyRepositoryBuilder GetAll(User? user, List<Consultation> consultations)
    {
        if(user is not null)
        {
            _mock.Setup(repository => repository.GetAll(user)).ReturnsAsync(consultations);
        }

        return this;
    }

    public ConsultationsReadOnlyRepositoryBuilder GetById(User user, Consultation? consultation)
    {
        if(consultation is not null)
        {
            _mock.Setup(repository => repository.GetById(user, It.IsAny<long>())).ReturnsAsync(consultation);
        }

        return this;
    }

    public ConsultationsReadOnlyRepositoryBuilder FilterServicesByMonth(User user, List<Consultation> consultations)
    {
        _mock.Setup(repository => repository.FilterConsultationsByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(consultations);

        return this;
    }

    public IConsultationsReadOnlyRepository Build() { return _mock.Object; }
}
