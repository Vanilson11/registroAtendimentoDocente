using FluentValidation.Results;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Domain.Security.Criptography;
using RegistroAtendimentoDocente.Domain.Security.Tokens;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUpdateOnlyUsersRepository _updateOnlyUsersRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUnitOffWork _unitOffWork;
    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IPasswordEncripter passwordEncripter,
        IUnitOffWork unitOffWork
        )
    {
        _loggedUser = loggedUser;
        _updateOnlyUsersRepository = updateOnlyUsersRepository;
        _passwordEncripter = passwordEncripter;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Password);

        var user = await _updateOnlyUsersRepository.GetById(loggedUser.Id);

        user!.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _updateOnlyUsersRepository.Update(user);

        await _unitOffWork.Commit();
    }

    private async Task Validate(RequestChangePasswordJson request, string currentPassword)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var passwordMatch = _passwordEncripter.Verify(request.Password, currentPassword);

        if(passwordMatch is false)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        if(result.IsValid is false)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}
