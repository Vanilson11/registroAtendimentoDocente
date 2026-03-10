namespace RegistroAtendimentoDocente.Communication.Responses;
public class ResponseRegisterAtendimentoJson
{
    public long Id { get; set; }
    public string Docente { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
}
