using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.GetById;
public class GetConsultationByIdUseCase : IGetConsultationByIdUseCase
{
    private readonly IConsultationsReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public GetConsultationByIdUseCase(IConsultationsReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseConsultationJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var consultation = await _repository.GetById(loggedUser, id);

        if (consultation is null) throw new NotFoundException(ResourceErrorMessages.CONSULTATION_NOT_FOUND);

        return _mapper.Map<ResponseConsultationJson>(consultation);
    }
}
