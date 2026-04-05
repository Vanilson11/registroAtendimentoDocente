using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Application.UseCases.Users.Update;
using RegistroAtendimentoDocente.Exception;
using Shouldly;

namespace Validator.Tests.UseCases.Users;

public class UpdateUserValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Role_Invalid()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build(role: "InvalidRole");

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.ROLE_INVALID));
    }
}
