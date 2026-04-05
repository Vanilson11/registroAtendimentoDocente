namespace RegistroAtendimentoDocente.Communication.Requests;
public class RequestConsultationJson
{
    public string Teacher {  get; set; } = string.Empty;
    public string Subject {  get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Recommendations {  get; set; } = string.Empty;
    public string Observation {  get; set; } = string.Empty;
}
