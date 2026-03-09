using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos;
using RegistroAtendimentoDocente.Exception;
using Shouldly;

namespace Validator.Tests.UseCases.Atendimentos.Register;
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
    public void Error_Teacher_Name_Empty(string teacher)
    {
        var validator = new AtendimentoUseCaseValidator();
        var request = new RequestRegisterAtendimentoJsonBuilder().Build();
        request.Docente = teacher;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.TEACHER_NAME_EMPTY));
    }

    [Fact]
    public void Error_Name_Less_Than_Two_Characters()
    {
        var validator = new AtendimentoUseCaseValidator();
        var request = new RequestRegisterAtendimentoJsonBuilder().Build();
        request.Docente = "a";

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MIN_CHARACTERS));
    }

    [Fact]
    public void Error_Name_More_Than_One_Hundred_Characters()
    {
        var validator = new AtendimentoUseCaseValidator();
        var request = new RequestRegisterAtendimentoJsonBuilder().Build();
        request.Docente = new string('A', 101);

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MAX_CHARACTERS));
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
