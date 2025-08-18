using FluentValidation;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento;
public class AtendimentoUseCaseValidator : AbstractValidator<RequestAtendimentoJson>
{
    public AtendimentoUseCaseValidator()
    {
        RuleFor(atendimento => atendimento.Docente).NotEmpty().WithMessage(ResourceErrorMessages.TEACHER_NAME_EMPTY);
        RuleFor(atendimento => atendimento.Assunto).NotEmpty().WithMessage(ResourceErrorMessages.SUBJECT_MEETING_EMPTY);
        RuleFor(atendimento => atendimento.Data).NotEmpty().WithMessage(ResourceErrorMessages.DATE_MEETING_EMPTY);
        RuleFor(atendimento => atendimento.Data).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.DATA_MEETING_INVALID);
    }
}
