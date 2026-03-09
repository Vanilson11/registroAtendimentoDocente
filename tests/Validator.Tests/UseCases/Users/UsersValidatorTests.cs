using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Application.UseCases.Users;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;

namespace Validator.Tests.UseCases.Users;
public class UsersValidatorTests
{
    private UsersValidator CreateValidator()
    {
        return new UsersValidator();
    }
    private RequestRegisterUserJson CreateRequest()
    {
        return RequestRegisterUserJsonBuilder.Build();
    }

    [Fact]
    public void Success()
    {
        var validator = CreateValidator();
        var request = CreateRequest();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = CreateValidator();
        var request = CreateRequest();
        request.Name = name;    

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.NAME_USER_EMPTY));
    }

    [Fact]
    public void Error_Name_Less_Than_Two_Characters()
    {
        var request = CreateRequest();
        request.Name = "a";
        var validator = CreateValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.NAME_MIN_CHARACTERS));
    }

    [Fact]
    public void Error_Name_More_Than_One_Hundred_Characters()
    {
        var request = CreateRequest();
        request.Name = new string('A', 101);
        var validator = CreateValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.NAME_MAX_CHARACTERS));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var validator = CreateValidator();
        var request = CreateRequest();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = CreateValidator();
        var request = CreateRequest();
        request.Email = "vanilson.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Password_Empty(string password)
    {
        var validator = CreateValidator();
        var request = CreateRequest();
        request.Password = password;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_EMPTY));
    }
}
