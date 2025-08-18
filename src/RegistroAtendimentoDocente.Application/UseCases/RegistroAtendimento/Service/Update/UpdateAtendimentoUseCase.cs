using AutoMapper;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Update;
public class UpdateAtendimentoUseCase : IUpdateAtendimentoUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IAtendimentoUpdateOnlyUseCase _repository;

    public UpdateAtendimentoUseCase(IMapper mapper, IUnitOffWork unitOffWork, IAtendimentoUpdateOnlyUseCase repository)
    {
        _mapper = mapper;
        _unitOffWork = unitOffWork;
        _repository = repository;
    }
    public async Task Execute(RequestAtendimentoJson request, int id)
    {
        Validate(request);

        var entity = await _repository.GetById(id);

        if (entity is null) throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

        _mapper.Map(request, entity);

        _repository.Update(entity);

        await _unitOffWork.Commit();
    }

    private void Validate(RequestAtendimentoJson request)
    {
        var validator = new AtendimentoUseCaseValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            var errorResponse = new ResponseAtendimentoErrorJson(errorMessages);

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
