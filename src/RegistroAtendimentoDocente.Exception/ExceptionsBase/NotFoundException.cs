
using System.Net;

namespace RegistroAtendimentoDocente.Exception.ExceptionsBase;
public class NotFoundException : RegistroAtendimentoDocenteException
{
    public NotFoundException(string message) : base(message) { }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
