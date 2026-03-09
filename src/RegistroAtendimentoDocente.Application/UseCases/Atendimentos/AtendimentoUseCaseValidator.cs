using FluentValidation;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos;
public class AtendimentoUseCaseValidator : AbstractValidator<RequestAtendimentoJson>
{
    public AtendimentoUseCaseValidator()
    {
        RuleFor(atendimento => atendimento.Docente).NotEmpty().WithMessage(ResourceErrorMessages.TEACHER_NAME_EMPTY)
            .MinimumLength(2).WithMessage(ResourceErrorMessages.NAME_MIN_CHARACTERS)
            .When(request => string.IsNullOrWhiteSpace(request.Docente) == false, ApplyConditionTo.CurrentValidator)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.NAME_MAX_CHARACTERS);
        RuleFor(atendimento => atendimento.Assunto).NotEmpty().WithMessage(ResourceErrorMessages.SUBJECT_MEETING_EMPTY);
        RuleFor(atendimento => atendimento.Data).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.DATA_MEETING_INVALID);
    }
}
