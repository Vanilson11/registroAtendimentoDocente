using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Atendimentos.Reports;

public class GetReportsTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "reports";
    private readonly string _tokenAdmin;
    private readonly string _tokenCoordenador1;
    private readonly string _tokenCoordinator2;
    private readonly DateTime _data;
    private readonly long _idCoordenador1;
    private readonly long _idCoordenador2;
    private readonly long _idAdmin;
    public GetReportsTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _tokenCoordenador1 = webApplicationFactory.User_Coordenador_1.GetToken();
        _tokenCoordinator2 = webApplicationFactory.User_Coordenador_2.GetToken();
        _data = webApplicationFactory.Atendimento_Coordenador.GetData();
        _idCoordenador1 = webApplicationFactory.User_Coordenador_1.GetId();
        _idCoordenador2 = webApplicationFactory.User_Coordenador_2.GetId();
        _idAdmin = webApplicationFactory.User_Admin.GetId();
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_data:yyyy-MM}", token: _tokenCoordenador1);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Success_Excel_Empty()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_data:yyyy-MM}", token: _tokenCoordinator2);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result.Content.Headers.ContentType.ShouldBeNull();
    }

    [Fact]
    public async Task Success_Excel_By_Coordenador()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel/coordenador/{_idCoordenador1}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Success_Empty_Excel_By_Coordenador()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel/coordenador/{_idCoordenador2}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result.Content.Headers.ContentType.ShouldBeNull();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_data:yyyy-MM}", token: _tokenCoordenador1);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Not_Found_Excel(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel/coordenador/100", token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Forbidden_User_Not_Coordinator_Excel(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel/coordenador/{_idAdmin}", token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_IS_NOT_COORDINATOR", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }


    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_data:yyyy-MM}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_data:yyyy-MM}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
