using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Users.Update;

public class UpdateUserTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users";
    private readonly string _tokenAdmin;
    private readonly string _token;
    private readonly string _email;
    private readonly long _id;
    public UpdateUserTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _token = webApplicationFactory.User_Others.GetToken();
        _email = webApplicationFactory.User_Coordenador.GetEmail();
        _id = webApplicationFactory.User_Others.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_id}", request: request, token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Not_Found(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _tokenAdmin, culture: culture);

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
    public async Task Error_Name_Empty(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(requestUri: $"{METHOD}/{_id}", request: request, token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_USER_EMPTY", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Email_Already_Registered(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = _email;

        var result = await DoPut(requestUri: $"{METHOD}/{_id}", request: request, token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_ALREADY_REGISTERED", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_id}", request: request);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_id}", request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
