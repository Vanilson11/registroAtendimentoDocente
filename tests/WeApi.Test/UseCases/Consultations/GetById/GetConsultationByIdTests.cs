using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Consultations.GetById;

public class GetConsultationByIdTests : RegisterConsultationTeacherDocenteClassFixture
{
    private const string METHOD = "consultation";
    private readonly string _token;
    private readonly string _tokenAdmin;
    private readonly Consultation _consultation;
    public GetConsultationByIdTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordinator_1.GetToken();
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _consultation = webApplicationFactory.Consultation_Coordinator.GetConsultation();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_consultation.Id}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_consultation.Id);
        response.RootElement.GetProperty("teacher").GetString().ShouldBe(_consultation.Teacher);
        response.RootElement.GetProperty("subject").GetString().ShouldBe(_consultation.Subject);
        response.RootElement.GetProperty("date").GetDateTime().ShouldBe(_consultation.Date);
        response.RootElement.GetProperty("recommendations").GetString().ShouldBe(_consultation.Recommendations);
        response.RootElement.GetProperty("observation").GetString().ShouldBe(_consultation.Observation);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Consultation_Not_Found(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

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
    public async Task Error_User_Unauthorized()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_consultation.Id}");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_consultation.Id}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
