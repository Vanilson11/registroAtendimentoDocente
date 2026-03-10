using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Domain.Security.Criptography;
using RegistroAtendimentoDocente.Domain.Security.Tokens;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Login.DoLogin;
public class DoLginUseCase : IDoLginUseCase
{
    private readonly IReadOnlyUsersRepository _usersRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLginUseCase(IReadOnlyUsersRepository usersRepository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        _usersRepository = usersRepository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }
    public async Task<ResponseRegisterUserJson> Execute(RequestDoLoginJson request)
    {
        var user = await _usersRepository.GetByEmail(request.Email);

        if(user is null)
        {
            throw new InvalidLoginException(ResourceErrorMessages.INVALID_LOGIN);
        }

        var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if(passwordMatch is false)
        {
            throw new InvalidLoginException(ResourceErrorMessages.INVALID_LOGIN);
        }

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}
