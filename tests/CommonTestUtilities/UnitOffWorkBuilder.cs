using Moq;
using RegistroAtendimentoDocente.Domain.Repositories;

namespace CommonTestUtilities;
public class UnitOffWorkBuilder
{
    public static IUnitOffWork Build()
    {
        var mock = new Mock<IUnitOffWork>();

        return mock.Object;
    }
}
