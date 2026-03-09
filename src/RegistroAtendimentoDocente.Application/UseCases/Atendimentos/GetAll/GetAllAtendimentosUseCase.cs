using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetAll;
public class GetAllAtendimentosUseCase : IGetAllAtendimentosUseCase
{
    private readonly IAtendimentosReadOnlyRepository _respository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetAllAtendimentosUseCase(IAtendimentosReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _respository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseAtendimentosJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _respository.GetAll(loggedUser);

        return new ResponseAtendimentosJson()
        {
            Atendimentos = _mapper.Map<List<ResponseShortAtendimentosJson>>(result)
        };
    }
}
