using FluentValidation;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Enums;
using RegistroAtendimentoDocente.Exception;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_USER_EMPTY)
            .MinimumLength(2).WithMessage(ResourceErrorMessages.NAME_MIN_CHARACTERS)
            .When(request => string.IsNullOrWhiteSpace(request.Name) == false, ApplyConditionTo.CurrentValidator)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.NAME_MAX_CHARACTERS);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID)
            .When(request => string.IsNullOrWhiteSpace(request.Email) == false, ApplyConditionTo.CurrentValidator);
        RuleFor(request => request.Role).NotEmpty().WithMessage(ResourceErrorMessages.ROLE_EMPTY)
            .Must(role => role == Roles.OUTROS || role == Roles.COORDENADOR || role == Roles.ADMIN)
            .When(request => string.IsNullOrWhiteSpace(request.Role) == false, ApplyConditionTo.CurrentValidator);
    }
}
