namespace RegistroAtendimentoDocente.Communication.Responses;
public class ResponseRegisterConsultationJson
{
    public long Id { get; set; }
    public string Teacher { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
}
