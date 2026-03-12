using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Users.Delete;
public class DeleteUserTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users";
    private readonly string _tokenAdmin;
    private readonly string _token;
    private readonly long _userId;
    private readonly string _email;
    private readonly string _password;
    public DeleteUserTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _token = webApplicationFactory.User_Others.GetToken();
        _userId = webApplicationFactory.User_Others.GetId();
        _email = webApplicationFactory.User_Others.GetEmail();
        _password = webApplicationFactory.User_Others.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_userId}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new RequestDoLoginJson()
        {
            Email = _email,
            Password = _password
        };

        result = await DoPost(requestUri: "login", request: loginRequest);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Not_Found(string culture)
    {
        var result = await DoDelete(requestUri: $"{METHOD}/100", token: _tokenAdmin, culture: culture);

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
        var result = await DoDelete(requestUri: $"{METHOD}/{_userId}");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_userId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
