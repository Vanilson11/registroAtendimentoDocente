namespace RegistroAtendimentoDocente.Domain.Entities;
public class Consultation
{
    public long Id { get; set; }
    public string Teacher { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime Date {  get; set; }
    public string? Recommendations {  get; set; }
    public string? Observation { get; set; }
    public long UserId { get; set; }
    public User User { get; set; } = default!;
}
