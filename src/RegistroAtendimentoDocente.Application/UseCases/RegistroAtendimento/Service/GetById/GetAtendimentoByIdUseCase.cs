using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.GetById;
public class GetAtendimentoByIdUseCase : IGetAtendimentoByIdUseCase
{
    private readonly IAtendimentosReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    public GetAtendimentoByIdUseCase(IAtendimentosReadOnlyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ResponseAtendimentoJson> Execute(int id)
    {
        var result = await _repository.GetById(id);

        if (result is null) throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

        return _mapper.Map<ResponseAtendimentoJson>(result);
    }
}
