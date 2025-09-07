namespace RegistroAtendimentoDocente.Domain.Entities;
public class Atendimento
{
    public int Id { get; set; }
    public string Docente { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
    public DateTime Data {  get; set; }
    public string? Encaminhamento {  get; set; }
    public string? Observacao { get; set; }
    public long UserId { get; set; }
    public User User { get; set; } = default!;
}
