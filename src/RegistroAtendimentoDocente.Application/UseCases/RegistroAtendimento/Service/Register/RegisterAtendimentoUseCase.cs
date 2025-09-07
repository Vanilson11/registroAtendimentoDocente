using AutoMapper;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Register;
public class RegisterAtendimentoUseCase : IRegisterAtendimentoUseCase
{
    private readonly IAtendimentoWriteOnlyRepository _atendimentoRepository;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IMapper _mapper;

    public RegisterAtendimentoUseCase(IAtendimentoWriteOnlyRepository atendimentoRepository, IUnitOffWork unitOffWork, IMapper mapper)
    {
        _unitOffWork = unitOffWork;
        _atendimentoRepository = atendimentoRepository;
        _mapper = mapper;
    }
    public async Task<ResponseRegisterAtendimentoJson> Execute(RequestAtendimentoJson request)
    {
        Validate(request);

        var entity = _mapper.Map<Atendimento>(request);

        await _atendimentoRepository.Add(entity);

        await _unitOffWork.Commit();

        return _mapper.Map<ResponseRegisterAtendimentoJson>(entity);
    }

    private void Validate(RequestAtendimentoJson request)
    {
        var validator = new AtendimentoUseCaseValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorList = result.Errors.Select(erro => erro.ErrorMessage).ToList();
            var errorResponse = new ResponseErrorsJson(errorList);

            throw new ErrorOnValidationException(errorList);
        }
    }
}
