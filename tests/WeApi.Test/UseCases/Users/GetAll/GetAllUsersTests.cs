using Shouldly;
using System.Net;
using System.Text.Json;

namespace WeApi.Test.UseCases.Users.GetAll;

public class GetAllUsersTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users";
    private readonly string _tokenAdmin;
    private readonly string _token;
    public GetAllUsersTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _token = webApplicationFactory.User_Others.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: METHOD, token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("users").EnumerateArray().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var result = await DoGet(requestUri: METHOD);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoGet(requestUri: METHOD, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
