namespace RegistroAtendimentoDocente.Exception.ExceptionsBase;
public abstract class RegistroAtendimentoDocenteException : SystemException
{
    protected RegistroAtendimentoDocenteException(string message) : base(message) { }

    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
