using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Atendimentos.GetById;

public class GetAtendimentoByIdTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "atendimento";
    private readonly string _token;
    private readonly string _tokenAdmin;
    private readonly Atendimento _atendimento;
    public GetAtendimentoByIdTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordenador_1.GetToken();
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _atendimento = webApplicationFactory.Atendimento_Coordenador.GetAtendimento();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_atendimento.Id}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_atendimento.Id);
        response.RootElement.GetProperty("docente").GetString().ShouldBe(_atendimento.Docente);
        response.RootElement.GetProperty("assunto").GetString().ShouldBe(_atendimento.Assunto);
        response.RootElement.GetProperty("data").GetDateTime().ShouldBe(_atendimento.Data);
        response.RootElement.GetProperty("encaminhamento").GetString().ShouldBe(_atendimento.Encaminhamento);
        response.RootElement.GetProperty("observacao").GetString().ShouldBe(_atendimento.Observacao);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Atendimento_Not_Found(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

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
        var result = await DoGet(requestUri: $"{METHOD}/{_atendimento.Id}");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_atendimento.Id}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
