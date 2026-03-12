
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.Delete;
public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUpdateOnlyUsersRepository _updateOnlyUsersRepository;
    private readonly IWriteOnlyUsersRepository _writeOnlyUsersRepository;
    private readonly IUnitOffWork _unitOffWork;
    public DeleteUserUseCase(
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IWriteOnlyUsersRepository writeOnlyUsersRepository,
        IUnitOffWork unitOffWork
        )
    {
        _updateOnlyUsersRepository = updateOnlyUsersRepository;
        _writeOnlyUsersRepository = writeOnlyUsersRepository;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(long id)
    {
        var user = await _updateOnlyUsersRepository.GetById(id);

        if (user is null) throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        await _writeOnlyUsersRepository.Delete(user.Id);

        await _unitOffWork.Commit();
    }
}
