using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestDoLoginJsonBuilder
{
    public static RequestDoLoginJson Build()
    {
        return new Faker<RequestDoLoginJson>()
            .RuleFor(request => request.Email, (faker) => faker.Internet.Email())
            .RuleFor(request => request.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
