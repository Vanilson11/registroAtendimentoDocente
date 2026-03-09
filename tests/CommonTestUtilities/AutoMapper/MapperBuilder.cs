using AutoMapper;
using RegistroAtendimentoDocente.Application.AutoMapper;

namespace CommonTestUtilities.AutoMapper;
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
