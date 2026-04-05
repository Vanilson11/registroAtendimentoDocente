using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(request => request.Name, faker => faker.Person.FirstName)
            .RuleFor(request => request.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(request => request.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
