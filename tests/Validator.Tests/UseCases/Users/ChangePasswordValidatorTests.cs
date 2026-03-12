using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Application.UseCases.Users.ChangePassword;
using RegistroAtendimentoDocente.Exception;
using Shouldly;

namespace Validator.Tests.UseCases.Users;

public class ChangePasswordValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_NewPassword_Empty(string password)
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = password;
        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.PASSWORD_EMPTY));
    }
}
