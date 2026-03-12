using Shouldly;
using System.Net;

namespace WeApi.Test.UseCases.Users.DeleteProfile;

public class DeleteUserProfileTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users/delete-profile";
    private readonly string _tokenCoordenador;
    public DeleteUserProfileTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenCoordenador = webApplicationFactory.User_Coordenador.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: METHOD, token: _tokenCoordenador);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var result = await DoDelete(requestUri: METHOD);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
