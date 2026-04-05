namespace RegistroAtendimentoDocente.Communication.Responses;
public class ResponseConsultationJson
{
    public long Id { get; set; }
    public string Teacher { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Recommendations { get; set; }
    public string? Observation { get; set; }
}
