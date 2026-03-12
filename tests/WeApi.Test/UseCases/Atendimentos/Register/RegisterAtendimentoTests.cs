using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Atendimentos.Register;
public class RegisterAtendimentoTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "atendimento";
    private readonly string _token;
    private readonly string _tokenAdmin;
    public RegisterAtendimentoTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordenador.GetToken();
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestAtendimentoJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.Created);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("docente").GetString().ShouldBe(request.Docente);
        response.RootElement.GetProperty("assunto").GetString().ShouldBe(request.Assunto);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Name_Docente_Empty(string culture)
    {
        var request = RequestAtendimentoJsonBuilder.Build();
        request.Docente = string.Empty;

        var result = await DoPost(requestUri: METHOD, request: request, token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TEACHER_NAME_EMPTY", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var request = RequestAtendimentoJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User() 
    {
        var request = RequestAtendimentoJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request, token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
