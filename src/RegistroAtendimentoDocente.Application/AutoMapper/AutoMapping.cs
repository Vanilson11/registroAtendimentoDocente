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
        CreateMap<RequestAtendimentoJson, Atendimento>();
    }

    private void EntityToResponse()
    {
        CreateMap<Atendimento, ResponseRegisterAtendimentoJson>();
        CreateMap<Atendimento, ResponseShortAtendimentosJson>();
        CreateMap<Atendimento, ResponseAtendimentoJson>();
    }
}
