using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Consultations.Update;

public class UpdateConsultationTests : RegisterConsultationTeacherDocenteClassFixture
{
    private const string METHOD = "consultation";
    private readonly string _token;
    private readonly string _tokenAdmin;
    private readonly long _consultationId;
    public UpdateConsultationTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordinator_1.GetToken();
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _consultationId = webApplicationFactory.Consultation_Coordinator.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestConsultationJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_consultationId}", request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{METHOD}/{_consultationId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_consultationId);
        response.RootElement.GetProperty("teacher").GetString().ShouldBe(request.Teacher);
        response.RootElement.GetProperty("subject").GetString().ShouldBe(request.Subject);
        response.RootElement.GetProperty("date").GetDateTime().ShouldBe(request.Date);
        response.RootElement.GetProperty("recommendations").GetString().ShouldBe(request.Recommendations);
        response.RootElement.GetProperty("observation").GetString().ShouldBe(request.Observation);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Name_Teacher_Empty(string culture)
    {
        var request = RequestConsultationJsonBuilder.Build();
        request.Teacher = string.Empty;

        var result = await DoPut(requestUri: $"{METHOD}/{_consultationId}", request: request, token: _token, culture: culture);

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
    public async Task Error_Consultation_Not_Found(string culture)
    {
        var request = RequestConsultationJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("CONSULTATION_NOT_FOUND", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var request = RequestConsultationJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_consultationId}", request: request);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var request = RequestConsultationJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_consultationId}", request: request, token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
