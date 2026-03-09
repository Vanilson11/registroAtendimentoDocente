using Moq;
using RegistroAtendimentoDocente.Domain.Repositories;

namespace CommonTestUtilities.Repositories;
public class UnitOffWorkBuilder
{
    public static IUnitOffWork Build()
    {
        var mock = new Mock<IUnitOffWork>();

        return mock.Object;
    }
}
