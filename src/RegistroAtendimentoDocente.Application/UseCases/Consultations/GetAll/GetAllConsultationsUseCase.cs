using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.GetAll;
public class GetAllConsultationsUseCase : IGetAllConsultationsUseCase
{
    private readonly IConsultationsReadOnlyRepository _respository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetAllConsultationsUseCase(IConsultationsReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _respository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseConsultationsJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var consultations = await _respository.GetAll(loggedUser);

        return new ResponseConsultationsJson()
        {
            Consultation = _mapper.Map<List<ResponseShortConsultationJson>>(consultations)
        };
    }
}
