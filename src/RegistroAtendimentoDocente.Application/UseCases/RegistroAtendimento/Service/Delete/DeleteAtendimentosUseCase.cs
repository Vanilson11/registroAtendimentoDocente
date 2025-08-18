
using RegistroAtendimentoDocente.Domain.Repositories;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Delete;
public class DeleteAtendimentosUseCase : IDeleteAtendimentosUseCase
{
    private readonly IAtendimentoWriteOnlyRepository _repository;
    private readonly IUnitOffWork _unitOffWork;

    public DeleteAtendimentosUseCase(IAtendimentoWriteOnlyRepository repository, IUnitOffWork unitOffWork)
    {
        _repository = repository;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(int id)
    {
        var result = await _repository.Delete(id);

        if (result is false) throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

        await _unitOffWork.Commit();
    }
}
