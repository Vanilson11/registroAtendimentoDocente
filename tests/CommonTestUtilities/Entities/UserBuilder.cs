using Bogus;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Enums;

namespace CommonTestUtilities.Entities;
public class UserBuilder
{
    public static List<User> Collection(uint count = 2)
    {
        var list = new List<User>();

        if (count == 0) count = 1;

        var userId = 1;

        for(int i = 0; i < count; i++)
        {
            var user = Build();

            user.Id = userId++;

            list.Add(user);
        }

        return list;
    }
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
