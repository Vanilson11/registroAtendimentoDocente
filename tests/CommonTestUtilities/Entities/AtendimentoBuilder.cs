using Bogus;
using RegistroAtendimentoDocente.Domain.Entities;

namespace CommonTestUtilities.Entities;
public class AtendimentoBuilder
{
    public static List<Atendimento> Collection(User user, uint count = 2)
    {
        var list = new List<Atendimento>();

        if (count == 0) count = 1;

        var atendimentoId = 1;

        for(int i = 0; i < count; i++)
        {
            var atendimento = Build(user);

            atendimento.Id = atendimentoId++;

            list.Add(atendimento);
        }

        return list;
    }

    public static Atendimento Build(User user)
    {
        return new Faker<Atendimento>()
            .RuleFor(a => a.Id, _ => 1)
            .RuleFor(a => a.Docente, faker => faker.Person.FirstName)
            .RuleFor(a => a.Assunto, faker => faker.Commerce.ProductName())
            .RuleFor(a => a.Data, faker => faker.Date.Past())
            .RuleFor(a => a.UserId, _ => user.Id);
    }
}
