using AutoMapper;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Entities;

namespace RegistroAtendimentoDocente.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestAtendimentoJson, Atendimento>()
            .ForMember(dest => dest.Docente, config => config.MapFrom(source => source.Docente));
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());
    }

    private void EntityToResponse()
    {
        CreateMap<Atendimento, ResponseRegisterAtendimentoJson>();
        CreateMap<Atendimento, ResponseShortAtendimentosJson>();
        CreateMap<Atendimento, ResponseAtendimentoJson>();
        CreateMap<User, ResponseShortUserJson>();
        CreateMap<User, ResponseUserJson>();
    }
}
