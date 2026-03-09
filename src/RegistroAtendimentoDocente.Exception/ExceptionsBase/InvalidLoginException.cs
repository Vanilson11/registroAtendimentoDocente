
using System.Net;

namespace RegistroAtendimentoDocente.Exception.ExceptionsBase;
public class InvalidLoginException : RegistroAtendimentoDocenteException
{
    public InvalidLoginException() : base(ResourceErrorMessages.INVALID_LOGIN)
    {
        
    }
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
