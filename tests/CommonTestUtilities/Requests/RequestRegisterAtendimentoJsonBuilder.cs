using Bogus;
using RegistroAtendimentoDocente.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRegisterAtendimentoJsonBuilder
{
    public static RequestAtendimentoJson Build()
    {
        return new Faker<RequestAtendimentoJson>()
            .RuleFor(atendimento => atendimento.Docente, faker => faker.Person.FirstName)
            .RuleFor(atendimento => atendimento.Assunto, faker => faker.Commerce.ProductDescription())
            .RuleFor(atendimento => atendimento.Data, faker => faker.Date.Past())
            .RuleFor(atendimento => atendimento.Encaminhamento, faker => faker.Commerce.ProductDescription())
            .RuleFor(atendimento => atendimento.Observacao, faker => faker.Commerce.ProductDescription());
    }
}
