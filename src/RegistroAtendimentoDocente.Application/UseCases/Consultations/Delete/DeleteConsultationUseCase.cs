using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.Consultations;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations.Delete;
public class DeleteConsultationUseCase : IDeleteConsultationUseCase
{
    private readonly IConsultationsWriteOnlyRepository _repository;
    private readonly IConsultationsReadOnlyRepository _repositoryReadOnly;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOffWork _unitOffWork;

    public DeleteConsultationUseCase(
        IConsultationsWriteOnlyRepository repository, 
        IUnitOffWork unitOffWork, 
        IConsultationsReadOnlyRepository repositoryReadOnly, 
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

        var consultation = await _repositoryReadOnly.GetById(loggedUser, id);

        if (consultation is null) throw new NotFoundException(ResourceErrorMessages.CONSULTATION_NOT_FOUND);

        await _repository.Delete(id);

        await _unitOffWork.Commit();
    }
}
