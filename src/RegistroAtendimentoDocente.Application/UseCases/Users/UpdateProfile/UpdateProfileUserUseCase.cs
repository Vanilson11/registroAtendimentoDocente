using FluentValidation.Results;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.UpdateProfile;

public class UpdateProfileUserUseCase : IUpdateProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUpdateOnlyUsersRepository _updateOnlyUsersRepository;
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    private readonly IUnitOffWork _unitOffWork;

    public UpdateProfileUserUseCase(
        ILoggedUser loggedUser,
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IReadOnlyUsersRepository readOnlyUsersRepository,
        IUnitOffWork unitOffWork
        )
    {
        _loggedUser = loggedUser;
        _updateOnlyUsersRepository = updateOnlyUsersRepository;
        _readOnlyUsersRepository = readOnlyUsersRepository;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(RequestUpdateUserProfileJson request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Email);

        var user = await _updateOnlyUsersRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _updateOnlyUsersRepository.Update(user);

        await _unitOffWork.Commit();
    }

    private async Task Validate(RequestUpdateUserProfileJson request, string currentEmail) 
    {
        var result = new UpdateProfileUserValidator().Validate(request);

        if(currentEmail.Equals(request.Email) == false)
        {
            var userWithEmailExists = await _readOnlyUsersRepository.ExistActiveUserWithEmail(request.Email);

            if (userWithEmailExists)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }
        }

        if(result.IsValid is false)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}
