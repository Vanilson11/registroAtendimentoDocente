using Bogus;
using RegistroAtendimentoDocente.Domain.Entities;

namespace CommonTestUtilities.Entities;
public class ConsultationBuilder
{
    public static List<Consultation> Collection(User user, uint count = 2)
    {
        var list = new List<Consultation>();

        if (count == 0) count = 1;

        var consultationId = 1;

        for(int i = 0; i < count; i++)
        {
            var consultation = Build(user);

            consultation.Id = consultationId++;

            list.Add(consultation);
        }

        return list;
    }

    public static Consultation Build(User user)
    {
        return new Faker<Consultation>()
            .RuleFor(c => c.Id, _ => 1)
            .RuleFor(c => c.Teacher, faker => faker.Person.FirstName)
            .RuleFor(c => c.Subject, faker => faker.Commerce.ProductName())
            .RuleFor(c => c.Date, faker => faker.Date.Past())
            .RuleFor(c => c.UserId, _ => user.Id);
    }
}
