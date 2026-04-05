using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Consultations.Reports;

public class GetReportsTests : RegisterConsultationTeacherDocenteClassFixture
{
    private const string METHOD = "reports";
    private readonly string _tokenAdmin;
    private readonly string _tokenCoordinator1;
    private readonly string _tokenCoordinator2;
    private readonly DateTime _data;
    private readonly long _idCoordinator1;
    private readonly long _idCoordinator2;
    private readonly long _idAdmin;
    public GetReportsTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _tokenCoordinator1 = webApplicationFactory.User_Coordinator_1.GetToken();
        _tokenCoordinator2 = webApplicationFactory.User_Coordinator_2.GetToken();
        _data = webApplicationFactory.Consultation_Coordinator.GetData();
        _idCoordinator1 = webApplicationFactory.User_Coordinator_1.GetId();
        _idCoordinator2 = webApplicationFactory.User_Coordinator_2.GetId();
        _idAdmin = webApplicationFactory.User_Admin.GetId();
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_data:yyyy-MM}", token: _tokenCoordinator1);

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
    public async Task Success_Excel_By_Coordinator()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel/coordinator/{_idCoordinator1}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Success_Empty_Excel_By_Coordinator()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel/coordinator/{_idCoordinator2}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result.Content.Headers.ContentType.ShouldBeNull();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_data:yyyy-MM}", token: _tokenCoordinator1);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_Pdf_Empty()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_data:yyyy-MM}", token: _tokenCoordinator2);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result.Content.Headers.ContentType.ShouldBeNull();
    }

    [Fact]
    public async Task Success_Pdf_By_Coordinator()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf/coordinator/{_idCoordinator1}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_Empty_Pdf_By_Coordinator()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf/coordinator/{_idCoordinator2}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result.Content.Headers.ContentType.ShouldBeNull();
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Not_Found_Excel(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel/coordinator/100", token: _tokenAdmin, culture: culture);

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
    public async Task Error_User_Not_Found_Pdf(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf/coordinator/100", token: _tokenAdmin, culture: culture);

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
        var result = await DoGet(requestUri: $"{METHOD}/excel/coordinator/{_idAdmin}", token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_IS_NOT_COORDINATOR", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Forbidden_User_Not_Coordinator_Pdf(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf/coordinator/{_idAdmin}", token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

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
