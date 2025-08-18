namespace RegistroAtendimentoDocente.Communication.Requests;
public class RequestAtendimentoJson
{
    public string Docente {  get; set; } = string.Empty;
    public string Assunto {  get; set; } = string.Empty;
    public string Encaminhamento {  get; set; } = string.Empty;
    public string Observacao {  get; set; } = string.Empty;
    public DateTime Data { get; set; }
}
