using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Application.UseCases.Consultations;
using RegistroAtendimentoDocente.Exception;
using Shouldly;

namespace Validator.Tests.UseCases.Consultations.Register;
public class RegisterConsultationUseCaseValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ConsultationUseCaseValidator();
        var request = RequestConsultationJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(true);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Teacher_Name_Empty(string teacher)
    {
        var validator = new ConsultationUseCaseValidator();
        var request = RequestConsultationJsonBuilder.Build();
        request.Teacher = teacher;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.TEACHER_NAME_EMPTY));
    }

    [Fact]
    public void Error_Name_Less_Than_Two_Characters()
    {
        var validator = new ConsultationUseCaseValidator();
        var request = RequestConsultationJsonBuilder.Build();
        request.Teacher = "a";

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MIN_CHARACTERS));
    }

    [Fact]
    public void Error_Name_More_Than_One_Hundred_Characters()
    {
        var validator = new ConsultationUseCaseValidator();
        var request = RequestConsultationJsonBuilder.Build();
        request.Teacher = new string('A', 101);

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MAX_CHARACTERS));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Subject_Empty(string assunto)
    {
        var validator = new ConsultationUseCaseValidator();
        var request = RequestConsultationJsonBuilder.Build();
        request.Subject = assunto;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.SUBJECT_CONSULTATION_EMPTY));
    }

    [Fact]
    public void Error_Date_Invalid()
    {
        var validator = new ConsultationUseCaseValidator();
        var request = RequestConsultationJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.DATE_CONSULTATION_INVALID));
    }
}
