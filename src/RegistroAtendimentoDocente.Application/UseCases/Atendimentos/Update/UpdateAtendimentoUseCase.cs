using AutoMapper;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Update;
public class UpdateAtendimentoUseCase : IUpdateAtendimentoUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IAtendimentoUpdateOnlyUseCase _repository;
    private readonly ILoggedUser _loggedUser;

    public UpdateAtendimentoUseCase(IMapper mapper, IUnitOffWork unitOffWork, IAtendimentoUpdateOnlyUseCase repository, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _unitOffWork = unitOffWork;
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task Execute(RequestAtendimentoJson request, long id)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var atendimento = await _repository.GetById(loggedUser, id);

        if (atendimento is null) throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

        _mapper.Map(request, atendimento);

        _repository.Update(atendimento);

        await _unitOffWork.Commit();
    }

    private void Validate(RequestAtendimentoJson request)
    {
        var validator = new AtendimentoUseCaseValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            var errorResponse = new ResponseErrorsJson(errorMessages);

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
