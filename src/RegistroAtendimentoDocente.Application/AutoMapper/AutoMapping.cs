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
        CreateMap<RequestConsultationJson, Consultation>()
            .ForMember(dest => dest.Teacher, config => config.MapFrom(source => source.Teacher));
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());
    }

    private void EntityToResponse()
    {
        CreateMap<Consultation, ResponseRegisterConsultationJson>();
        CreateMap<Consultation, ResponseShortConsultationJson>();
        CreateMap<Consultation, ResponseConsultationJson>();
        CreateMap<User, ResponseShortUserJson>();
        CreateMap<User, ResponseUserJson>();
    }
}
