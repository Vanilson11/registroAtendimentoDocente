
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.DeleteProfile;

public class DeleteProfileUserUseCase : IDeleteProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IWriteOnlyUsersRepository _writeOnlyUsersRepository;
    private readonly IUpdateOnlyUsersRepository _updateOnlyUsersRepository;
    private readonly IUnitOffWork _unitOffWork;
    public DeleteProfileUserUseCase(
        ILoggedUser loggedUser,
        IWriteOnlyUsersRepository writeOnlyUsersRepository,
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IUnitOffWork unitOffWork
        )
    {
        _loggedUser = loggedUser;
        _writeOnlyUsersRepository = writeOnlyUsersRepository;
        _updateOnlyUsersRepository = updateOnlyUsersRepository;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var user = await _updateOnlyUsersRepository.GetById(loggedUser.Id);

        await _writeOnlyUsersRepository.Delete(user!.Id);

        await _unitOffWork.Commit();
    }
}
