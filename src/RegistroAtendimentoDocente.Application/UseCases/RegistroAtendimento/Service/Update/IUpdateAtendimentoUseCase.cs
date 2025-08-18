using RegistroAtendimentoDocente.Communication.Requests;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Update;
public interface IUpdateAtendimentoUseCase
{
    Task Execute(RequestAtendimentoJson request, int id);
}
