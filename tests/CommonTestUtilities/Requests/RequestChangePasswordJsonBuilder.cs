using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(request => request.Password, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(request => request.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
