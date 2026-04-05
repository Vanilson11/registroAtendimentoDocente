using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Enums;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build(string role = Roles.COORDINATOR)
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Role, _ => role);
    }
}
