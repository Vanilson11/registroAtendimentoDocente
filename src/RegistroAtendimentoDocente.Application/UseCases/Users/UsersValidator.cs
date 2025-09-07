using FluentValidation;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;

namespace RegistroAtendimentoDocente.Application.UseCases.Users;
public class UsersValidator : AbstractValidator<RequestRegisterUserJson>
{
    public UsersValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_USER_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_EMPTY);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}
