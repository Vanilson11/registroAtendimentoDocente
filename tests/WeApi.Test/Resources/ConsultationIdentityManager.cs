using RegistroAtendimentoDocente.Domain.Entities;

namespace WeApi.Test.Resources;
public class ConsultationIdentityManager
{
    private readonly Consultation _consultation;

    public ConsultationIdentityManager(Consultation consultation)
    {
        _consultation = consultation;
    }

    public long GetId() => _consultation.Id;
    public Consultation GetConsultation() => _consultation;
    public DateTime GetData() => _consultation.Date;
}
