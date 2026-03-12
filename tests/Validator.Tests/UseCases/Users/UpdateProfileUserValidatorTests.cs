using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Application.UseCases.Users.UpdateProfile;
using RegistroAtendimentoDocente.Exception;
using Shouldly;

namespace Validator.Tests.UseCases.Users;

public class UpdateProfileUserValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        
        var result = new UpdateProfileUserValidator().Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = name;

        var result = new UpdateProfileUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_USER_EMPTY));
    }

    [Fact]
    public void Error_Name_Less_Tha_Two_Characters()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = "a";

        var result = new UpdateProfileUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MIN_CHARACTERS));
    }

    [Fact]
    public void Error_Name_More_Than_One_Hundred_Characters()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = new string('A', 101);

        var result = new UpdateProfileUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MAX_CHARACTERS));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = email;

        var result = new UpdateProfileUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = "invalid-email.com";

        var result = new UpdateProfileUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID));
    }
}
