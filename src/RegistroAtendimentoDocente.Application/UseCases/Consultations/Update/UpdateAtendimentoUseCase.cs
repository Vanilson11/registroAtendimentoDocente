using AutoMapper;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Update;
public class UpdateAtendimentoUseCase : IUpdateConsultationUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IConsultationsUpdateOnlyUseCase _repository;
    private readonly ILoggedUser _loggedUser;

    public UpdateAtendimentoUseCase(IMapper mapper, IUnitOffWork unitOffWork, IConsultationsUpdateOnlyUseCase repository, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _unitOffWork = unitOffWork;
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task Execute(RequestConsultationJson request, long id)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var consultation = await _repository.GetById(loggedUser, id);

        if (consultation is null) throw new NotFoundException(ResourceErrorMessages.CONSULTATION_NOT_FOUND);

        _mapper.Map(request, consultation);

        _repository.Update(consultation);

        await _unitOffWork.Commit();
    }

    private void Validate(RequestConsultationJson request)
    {
        var validator = new ConsultationUseCaseValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
