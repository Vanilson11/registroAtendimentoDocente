using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Delete;
public class DeleteAtendimentosUseCase : IDeleteAtendimentosUseCase
{
    private readonly IAtendimentoWriteOnlyRepository _repository;
    private readonly IAtendimentosReadOnlyRepository _repositoryReadOnly;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOffWork _unitOffWork;

    public DeleteAtendimentosUseCase(
        IAtendimentoWriteOnlyRepository repository, 
        IUnitOffWork unitOffWork, 
        IAtendimentosReadOnlyRepository repositoryReadOnly, 
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOffWork = unitOffWork;
        _repositoryReadOnly = repositoryReadOnly;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var atendimento = await _repositoryReadOnly.GetById(loggedUser, id);

        if (atendimento is null) throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

        await _repository.Delete(id);

        await _unitOffWork.Commit();
    }
}
