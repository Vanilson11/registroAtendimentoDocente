using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Users.GetById;

public class GetUserByIdTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users";
    private readonly string _tokenAdmin;
    private readonly string _token;
    private readonly User _user;
    public GetUserByIdTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _token = webApplicationFactory.User_Others.GetToken();
        _user = webApplicationFactory.User_Others.GetUser();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_user.Id}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_user.Id);
        response.RootElement.GetProperty("name").GetString().ShouldBe(_user.Name);
        response.RootElement.GetProperty("email").GetString().ShouldBe(_user.Email);
        response.RootElement.GetProperty("role").GetString().ShouldBe(_user.Role);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Not_Found(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/1000", token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_user.Id}");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_user.Id}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
