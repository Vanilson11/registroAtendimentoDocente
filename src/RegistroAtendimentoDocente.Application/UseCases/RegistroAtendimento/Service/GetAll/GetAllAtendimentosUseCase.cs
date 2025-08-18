using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.GetAll;
public class GetAllAtendimentosUseCase : IGetAllAtendimentosUseCase
{
    private readonly IAtendimentosReadOnlyRepository _respository;
    private readonly IMapper _mapper;

    public GetAllAtendimentosUseCase(IAtendimentosReadOnlyRepository repository, IMapper mapper)
    {
        _respository = repository;
        _mapper = mapper;
    }
    public async Task<ResponseAtendimentosJson> Execute()
    {
        var result = await _respository.GetAll();

        return new ResponseAtendimentosJson()
        {
            Atendimentos = _mapper.Map<List<ResponseShortAtendimentosJson>>(result)
        };
    }
}
