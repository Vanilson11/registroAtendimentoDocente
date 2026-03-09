using AutoMapper;
using RegistroAtendimentoDocente.Application.AutoMapper;

namespace CommonTestUtilities;
public class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapping());
        });

        return mapper.CreateMapper();
    }
}
