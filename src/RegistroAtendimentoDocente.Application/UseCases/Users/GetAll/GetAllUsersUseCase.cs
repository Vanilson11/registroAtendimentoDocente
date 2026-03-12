using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Users;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.GetAll;

public class GetAllUsersUseCase : IGetAllUsersUseCase
{
    private readonly IMapper _mapper;
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    public GetAllUsersUseCase(IMapper mapper, IReadOnlyUsersRepository readOnlyUsersRepository)
    {
        _mapper = mapper;
        _readOnlyUsersRepository = readOnlyUsersRepository;
    }
    public async Task<ResponseUsersJson> Execute()
    {
        var users = await _readOnlyUsersRepository.GetAll();

        return new ResponseUsersJson()
        {
            Users = _mapper.Map<List<ResponseShortUserJson>>(users)
        };
    }
}
