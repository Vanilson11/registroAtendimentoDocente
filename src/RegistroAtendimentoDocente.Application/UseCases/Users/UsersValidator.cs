using FluentValidation;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;

namespace RegistroAtendimentoDocente.Application.UseCases.Users;
public class UsersValidator : AbstractValidator<RequestRegisterUserJson>
{
    public UsersValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_USER_EMPTY)
            .MinimumLength(2).WithMessage(ResourceErrorMessages.NAME_MIN_CHARACTERS)
            .When(request => string.IsNullOrWhiteSpace(request.Name) == false, ApplyConditionTo.CurrentValidator)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.NAME_MAX_CHARACTERS);
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(request => string.IsNullOrWhiteSpace(request.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(request => request.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}
