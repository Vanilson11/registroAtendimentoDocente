using Shouldly;
using System.Text.Json;

namespace WeApi.Test.UseCases.Users.GetAllCoordinators;

public class GetAllCoordinatorsTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users";
    private readonly string _token;
    public GetAllCoordinatorsTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Others.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/get-all-coordinators", token: _token);

        result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("users").EnumerateArray().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var result = await DoGet(requestUri: $"{METHOD}/get-all-coordinators");

        result.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
    }
}
