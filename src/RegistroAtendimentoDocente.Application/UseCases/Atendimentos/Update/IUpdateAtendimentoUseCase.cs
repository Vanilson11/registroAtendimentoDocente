using RegistroAtendimentoDocente.Communication.Requests;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Update;
public interface IUpdateAtendimentoUseCase
{
    Task Execute(RequestAtendimentoJson request, long id);
}
