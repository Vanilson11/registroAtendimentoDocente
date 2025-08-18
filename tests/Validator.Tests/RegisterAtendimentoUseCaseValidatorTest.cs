using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento;
using RegistroAtendimentoDocente.Exception;
using Shouldly;

namespace Validator.Tests;
public class RegisterAtendimentoUseCaseValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new AtendimentoUseCaseValidator();
        var request = new RequestRegisterAtendimentoJsonBuilder().Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(true);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Teacher_Empty(string teacher)
    {
        var validator = new AtendimentoUseCaseValidator();
        var request = new RequestRegisterAtendimentoJsonBuilder().Build();
        request.Docente = teacher;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.TEACHER_NAME_EMPTY));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Assunto_Empty(string assunto)
    {
        var validator = new AtendimentoUseCaseValidator();
        var request = new RequestRegisterAtendimentoJsonBuilder().Build();
        request.Assunto = assunto;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.SUBJECT_MEETING_EMPTY));
    }

    [Fact]
    public void Error_Date_Invalid()
    {
        var validator = new AtendimentoUseCaseValidator();
        var request = new RequestRegisterAtendimentoJsonBuilder().Build();
        request.Data = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.DATA_MEETING_INVALID));
    }
}
