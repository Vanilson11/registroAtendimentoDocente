using CommonTestUtilities.Requests;
using FluentValidation;
using RegistroAtendimentoDocente.Application.UseCases.Users;
using RegistroAtendimentoDocente.Communication.Requests;
using Shouldly;

namespace Validator.Tests.UseCases.Users;
public class PasswordValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void Error_Password_Invalid(string password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = password;

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        result.ShouldBeFalse();
    }
}
