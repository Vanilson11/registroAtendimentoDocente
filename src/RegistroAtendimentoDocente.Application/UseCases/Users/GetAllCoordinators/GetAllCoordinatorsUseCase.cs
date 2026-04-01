using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Users;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.GetAllCoordinators;

public class GetAllCoordinatorsUseCase : IGetAllCoordinatorsUseCase
{
    private readonly IReadOnlyUsersRepository _usersRepository;
    private readonly IMapper _mapper;
    public GetAllCoordinatorsUseCase(IReadOnlyUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }
    public async Task<ResponseUsersJson> Execute()
    {
        var coordinators = await _usersRepository.GetAllCoordinators();

        return new ResponseUsersJson
        {
            Users = _mapper.Map<List<ResponseShortUserJson>>(coordinators)
        };
    }
}
