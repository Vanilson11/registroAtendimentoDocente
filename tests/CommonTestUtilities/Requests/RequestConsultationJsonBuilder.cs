using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestConsultationJsonBuilder
{
    public static RequestConsultationJson Build()
    {
        return new Faker<RequestConsultationJson>()
            .RuleFor(consultation => consultation.Teacher, faker => faker.Person.FirstName)
            .RuleFor(consultation => consultation.Subject, faker => faker.Commerce.ProductDescription())
            .RuleFor(consultation => consultation.Date, faker => faker.Date.Past())
            .RuleFor(consultation => consultation.Recommendations, faker => faker.Commerce.ProductDescription())
            .RuleFor(consultation => consultation.Observation, faker => faker.Commerce.ProductDescription());
    }
}
