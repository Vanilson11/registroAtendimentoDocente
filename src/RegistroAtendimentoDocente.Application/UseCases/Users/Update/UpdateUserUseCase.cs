using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUpdateOnlyUsersRepository _updateOnlyUsersRepository;
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    private readonly IUnitOffWork _unitOffWork;
    public UpdateUserUseCase(
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IReadOnlyUsersRepository readOnlyUsersRepository,
        IUnitOffWork unitOffWork
        )
    {
        _updateOnlyUsersRepository = updateOnlyUsersRepository;
        _readOnlyUsersRepository = readOnlyUsersRepository;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(RequestUpdateUserJson request, long id)
    {
        var user = await _updateOnlyUsersRepository.GetById(id);

        if (user is null) throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        await Validate(request, user.Email);

        user.Name = request.Name;
        user.Email = request.Email;
        user.Role = request.Role;

        _updateOnlyUsersRepository.Update(user);

        await _unitOffWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var result = new UpdateUserValidator().Validate(request);

        if(currentEmail.Equals(request.Email) == false)
        {
            var userWithExistsEmail = await _readOnlyUsersRepository.ExistActiveUserWithEmail(request.Email);

            if (userWithExistsEmail)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }
        }

        if(result.IsValid is false)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}
