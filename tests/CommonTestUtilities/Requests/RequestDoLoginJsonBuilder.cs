using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestDoLoginJsonBuilder
{
    public static RequestDoLoginJson Build()
    {
        return new Faker<RequestDoLoginJson>()
            .RuleFor(user => user.Email, (faker) => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
