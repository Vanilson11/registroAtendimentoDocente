using AutoMapper;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.GetById;

public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    private readonly IMapper _mapper;
    public GetUserByIdUseCase(IReadOnlyUsersRepository readOnlyusersRepository, IMapper mapper)
    {
        _readOnlyUsersRepository = readOnlyusersRepository;
        _mapper = mapper;
    }
    public async Task<ResponseUserJson> Execute(long id)
    {
        var user = await _readOnlyUsersRepository.GetById(id);

        if (user is null) throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        return _mapper.Map<ResponseUserJson>(user);
    }
}
