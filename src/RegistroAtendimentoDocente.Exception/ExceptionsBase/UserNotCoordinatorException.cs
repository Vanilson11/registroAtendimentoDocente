
using System.Net;

namespace RegistroAtendimentoDocente.Exception.ExceptionsBase;

public class UserNotCoordinatorException : RegistroAtendimentoDocenteException
{
    public UserNotCoordinatorException(string message) : base(message)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Forbidden;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
