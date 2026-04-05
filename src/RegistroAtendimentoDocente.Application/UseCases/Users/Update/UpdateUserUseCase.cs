using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUpdateOnlyUsersRepository _updateOnlyUsersRepository;
    private readonly IUnitOffWork _unitOffWork;
    public UpdateUserUseCase(
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IUnitOffWork unitOffWork
        )
    {
        _updateOnlyUsersRepository = updateOnlyUsersRepository;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(RequestUpdateUserJson request, long id)
    {
        await Validate(request);

        var user = await _updateOnlyUsersRepository.GetById(id);

        if (user is null) throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        user.Role = request.Role;

        _updateOnlyUsersRepository.Update(user);

        await _unitOffWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request)
    {
        var result = new UpdateUserValidator().Validate(request);

        if(result.IsValid is false)
        {
            var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}
