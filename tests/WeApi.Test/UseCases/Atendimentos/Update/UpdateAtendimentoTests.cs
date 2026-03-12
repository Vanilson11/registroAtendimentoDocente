using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Atendimentos.Update;

public class UpdateAtendimentoTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "atendimento";
    private readonly string _token;
    private readonly string _tokenAdmin;
    private readonly long _atendimentoId;
    public UpdateAtendimentoTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordenador.GetToken();
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _atendimentoId = webApplicationFactory.Atendimento_Coordenador.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestAtendimentoJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_atendimentoId}", request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{METHOD}/{_atendimentoId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_atendimentoId);
        response.RootElement.GetProperty("docente").GetString().ShouldBe(request.Docente);
        response.RootElement.GetProperty("assunto").GetString().ShouldBe(request.Assunto);
        response.RootElement.GetProperty("data").GetDateTime().ShouldBe(request.Data);
        response.RootElement.GetProperty("encaminhamento").GetString().ShouldBe(request.Encaminhamento);
        response.RootElement.GetProperty("observacao").GetString().ShouldBe(request.Observacao);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Name_Docente_Empty(string culture)
    {
        var request = RequestAtendimentoJsonBuilder.Build();
        request.Docente = string.Empty;

        var result = await DoPut(requestUri: $"{METHOD}/{_atendimentoId}", request: request, token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TEACHER_NAME_EMPTY", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Atendimento_Not_Found(string culture)
    {
        var request = RequestAtendimentoJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("SERVICE_NOT_FOUND", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var request = RequestAtendimentoJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_atendimentoId}", request: request);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var request = RequestAtendimentoJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_atendimentoId}", request: request, token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
