using AutoMapper;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Register;
public class RegisterAtendimentoUseCase : IRegisterAtendimentoUseCase
{
    private readonly IAtendimentoWriteOnlyRepository _atendimentoRepository;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public RegisterAtendimentoUseCase(IAtendimentoWriteOnlyRepository atendimentoRepository, IUnitOffWork unitOffWork, IMapper mapper, ILoggedUser loggedUser)
    {
        _unitOffWork = unitOffWork;
        _atendimentoRepository = atendimentoRepository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRegisterAtendimentoJson> Execute(RequestAtendimentoJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var atendimento = _mapper.Map<Atendimento>(request);

        atendimento.UserId = loggedUser.Id;

        await _atendimentoRepository.Add(atendimento);

        await _unitOffWork.Commit();

        return _mapper.Map<ResponseRegisterAtendimentoJson>(atendimento);
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
