using Bogus;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Enums;

namespace CommonTestUtilities.Entities;
public class UserBuilder
{
    public static User Build(string role = Roles.COORDENADOR)
    {
        return new Faker<User>()
            .RuleFor(user => user.Id, _ => 1)
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(user => user.Role, _ => role)
            .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid());
    }
}
