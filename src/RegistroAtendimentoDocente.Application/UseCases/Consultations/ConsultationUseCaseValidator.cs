using FluentValidation;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;

namespace RegistroAtendimentoDocente.Application.UseCases.Consultations;
public class ConsultationUseCaseValidator : AbstractValidator<RequestConsultationJson>
{
    public ConsultationUseCaseValidator()
    {
        RuleFor(request => request.Teacher).NotEmpty().WithMessage(ResourceErrorMessages.TEACHER_NAME_EMPTY)
            .MinimumLength(2).WithMessage(ResourceErrorMessages.NAME_MIN_CHARACTERS)
            .When(request => string.IsNullOrWhiteSpace(request.Teacher) == false, ApplyConditionTo.CurrentValidator)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.NAME_MAX_CHARACTERS);
        RuleFor(request => request.Subject).NotEmpty().WithMessage(ResourceErrorMessages.SUBJECT_CONSULTATION_EMPTY);
        RuleFor(request => request.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.DATE_CONSULTATION_INVALID);
    }
}
