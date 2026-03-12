using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.GetProfile;

public class GetProfileUserUseCase : IGetProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IReadOnlyUsersRepository _readOnlyusersRepository;
    private readonly IMapper _mapper;

    public GetProfileUserUseCase(ILoggedUser loggedUser, IReadOnlyUsersRepository readOnlyusersRepository, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _readOnlyusersRepository = readOnlyusersRepository;
        _mapper = mapper;
    }
    public async Task<ResponseShortUserJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var user = await _readOnlyusersRepository.GetById(loggedUser.Id);

        return _mapper.Map<ResponseShortUserJson>(user);
    }
}
