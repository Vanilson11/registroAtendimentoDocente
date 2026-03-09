using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetById;
public class GetAtendimentoByIdUseCase : IGetAtendimentoByIdUseCase
{
    private readonly IAtendimentosReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public GetAtendimentoByIdUseCase(IAtendimentosReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseAtendimentoJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetById(loggedUser, id);

        if (result is null) throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

        return _mapper.Map<ResponseAtendimentoJson>(result);
    }
}
