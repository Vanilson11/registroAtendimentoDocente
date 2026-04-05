using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Consultations.Delete;

public class DeleteConsultationTests : RegisterConsultationTeacherDocenteClassFixture
{
    private const string METHOD = "consultation";
    private readonly string _token;
    private readonly string _tokenUserOthers;
    private readonly long _consultationId;
    public DeleteConsultationTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordinator_1.GetToken();
        _tokenUserOthers = webApplicationFactory.User_Others.GetToken();
        _consultationId = webApplicationFactory.Consultation_Coordinator.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_consultationId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{METHOD}/{_consultationId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Consultation_Not_Found(string culture)
    {
        var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

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
        var result = await DoDelete(requestUri: $"{METHOD}/{_consultationId}");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_consultationId}", token: _tokenUserOthers);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
