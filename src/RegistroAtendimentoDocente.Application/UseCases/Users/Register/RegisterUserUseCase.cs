using AutoMapper;
using FluentValidation.Results;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.UsersRepository;
using RegistroAtendimentoDocente.Domain.Security.Criptography;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IReadOnlyUsersRepository _userReadOnlyRepository;
    private readonly IWriteOnlyUsersRepository _userWriteOnlyRepositoy;
    private readonly IUnitOffWork _unitOffWork;

    public RegisterUserUseCase(IMapper mapper,
        IPasswordEncripter passwordEncripter,
        IReadOnlyUsersRepository userReadOnlyRepository,
        IWriteOnlyUsersRepository userWriteOnlyRepositoy,
        IUnitOffWork unitOffWork)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepositoy = userWriteOnlyRepositoy;
        _unitOffWork = unitOffWork;
    }
    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _userWriteOnlyRepositoy.Add(user);

        await _unitOffWork.Commit();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
        };
    }

    public async Task Validate(RequestRegisterUserJson request)
    {
        var result = new UsersValidator().Validate(request);

        var emailExists = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(errorMessage => errorMessage.ErrorMessage).ToList();
            var errorResponse = new ResponseErrorsJson(errorMessages);

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
