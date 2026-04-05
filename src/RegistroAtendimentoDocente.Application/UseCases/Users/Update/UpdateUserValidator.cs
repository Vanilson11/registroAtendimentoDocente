using FluentValidation;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Enums;
using RegistroAtendimentoDocente.Exception;

namespace RegistroAtendimentoDocente.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Role).NotEmpty().WithMessage(ResourceErrorMessages.ROLE_EMPTY)
            .Must(role => role == Roles.OTHERS || role == Roles.COORDINATOR || role == Roles.ADMIN).WithMessage(ResourceErrorMessages.ROLE_INVALID)
            .When(request => string.IsNullOrWhiteSpace(request.Role) == false, ApplyConditionTo.CurrentValidator);
    }
}
