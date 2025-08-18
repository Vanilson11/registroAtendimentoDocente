namespace RegistroAtendimentoDocente.Communication.Responses;
public class ResponseAtendimentoErrorJson
{
    public List<string> ErrorMessages { get; set; }
    public ResponseAtendimentoErrorJson(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }

    public ResponseAtendimentoErrorJson(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
