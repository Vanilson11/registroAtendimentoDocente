using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserProfileJsonBuilder
{
    public static RequestUpdateUserProfileJson Build()
    {
        return new Faker<RequestUpdateUserProfileJson>()
            .RuleFor(request => request.Name, faker => faker.Person.FirstName)
            .RuleFor(request => request.Email, (faker, request) => faker.Internet.Email(request.Name));
    }
}
