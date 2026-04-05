using AutoMapper;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Register;
public class RegisterConsultationUseCase : IRegisterConsultationUseCase
{
    private readonly IConsultationsWriteOnlyRepository _consultationWriteOnlyRepository;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public RegisterConsultationUseCase(
        IConsultationsWriteOnlyRepository consultationWriteOnlyRepository, 
        IUnitOffWork unitOffWork, 
        IMapper mapper, 
        ILoggedUser loggedUser)
    {
        _unitOffWork = unitOffWork;
        _consultationWriteOnlyRepository = consultationWriteOnlyRepository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRegisterConsultationJson> Execute(RequestConsultationJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var consultation = _mapper.Map<Consultation>(request);

        consultation.UserId = loggedUser.Id;

        await _consultationWriteOnlyRepository.Add(consultation);

        await _unitOffWork.Commit();

        return _mapper.Map<ResponseRegisterConsultationJson>(consultation);
    }

    private void Validate(RequestConsultationJson request)
    {
        var validator = new ConsultationUseCaseValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorList = result.Errors.Select(erro => erro.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorList);
        }
    }
}
