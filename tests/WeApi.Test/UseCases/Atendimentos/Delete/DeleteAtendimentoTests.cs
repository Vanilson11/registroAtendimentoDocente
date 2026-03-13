using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Atendimentos.Delete;

public class DeleteAtendimentoTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "atendimento";
    private readonly string _token;
    private readonly string _tokenUserOthers;
    private readonly long _atendimentoId;
    public DeleteAtendimentoTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordenador_1.GetToken();
        _tokenUserOthers = webApplicationFactory.User_Others.GetToken();
        _atendimentoId = webApplicationFactory.Atendimento_Coordenador.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_atendimentoId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{METHOD}/{_atendimentoId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Atendimento_Not_Found(string culture)
    {
        var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

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
    public async Task Error_User_Unauthorized()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_atendimentoId}");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_atendimentoId}", token: _tokenUserOthers);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
