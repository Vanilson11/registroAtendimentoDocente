using Shouldly;
using System.Net;

namespace WeApi.Test.UseCases.Consultations.GetAll;

public class GetAllConsultationsTests : RegisterConsultationTeacherDocenteClassFixture
{
    private const string METHOD = "consultation";
    private readonly string _token;
    private readonly string _tokenAdmin;
    public GetAllConsultationsTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordinator_1.GetToken();
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: METHOD, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Error_User_Unauthorized()
    {
        var result = await DoGet(requestUri: METHOD);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoGet(requestUri: METHOD, token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
