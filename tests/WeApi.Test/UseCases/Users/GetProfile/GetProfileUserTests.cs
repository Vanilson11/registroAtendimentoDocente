using RegistroAtendimentoDocente.Domain.Entities;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WeApi.Test.UseCases.Users.GetProfile;

public class GetProfileUserTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users/get-profile";
    private readonly string _tokenCoordenador;
    private readonly User _userCoordenador;
    public GetProfileUserTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenCoordenador = webApplicationFactory.User_Coordenador_1.GetToken();
        _userCoordenador = webApplicationFactory.User_Coordenador_1.GetUser();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: METHOD, token: _tokenCoordenador);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(_userCoordenador.Name);
        response.RootElement.GetProperty("email").GetString().ShouldBe(_userCoordenador.Email);
        response.RootElement.GetProperty("role").GetString().ShouldBe(_userCoordenador.Role);
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var result = await DoGet(requestUri: METHOD);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
