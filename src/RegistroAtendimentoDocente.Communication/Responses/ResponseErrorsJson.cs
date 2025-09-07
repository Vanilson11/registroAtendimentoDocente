namespace RegistroAtendimentoDocente.Communication.Responses;
public class ResponseErrorsJson
{
    public List<string> ErrorMessages { get; set; }
    public ResponseErrorsJson(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }

    public ResponseErrorsJson(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
