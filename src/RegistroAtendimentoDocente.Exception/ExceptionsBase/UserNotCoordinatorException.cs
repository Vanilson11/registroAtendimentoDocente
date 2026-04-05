
using System.Net;

namespace RegistroAtendimentoDocente.Exception.ExceptionsBase;

public class UserNotCoordinatorException : RegisterConsultationTeacherException
{
    public UserNotCoordinatorException(string message) : base(message)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
