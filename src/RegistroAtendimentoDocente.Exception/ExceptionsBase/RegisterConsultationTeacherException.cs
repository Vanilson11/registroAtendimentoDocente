namespace RegistroAtendimentoDocente.Exception.ExceptionsBase;
public abstract class RegisterConsultationTeacherException : SystemException
{
    protected RegisterConsultationTeacherException(string message) : base(message) { }

    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
