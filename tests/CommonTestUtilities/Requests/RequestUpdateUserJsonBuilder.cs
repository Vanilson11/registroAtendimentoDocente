using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Enums;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build(string role = Roles.COORDENADOR)
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, faker => faker.Person.FirstName)
            .RuleFor(request => request.Email, (faker, request) => faker.Internet.Email(request.Name))
            .RuleFor(request => request.Role, _ => role);
    }
}
