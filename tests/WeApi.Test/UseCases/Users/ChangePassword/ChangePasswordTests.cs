using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Users.ChangePassword;

public class ChangePasswordTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users/change-password";
    private readonly string _token;
    private readonly string _password;
    private readonly string _email;
    public ChangePasswordTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Others.GetToken();
        _password = webApplicationFactory.User_Others.GetPassword();
        _email = webApplicationFactory.User_Others.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var result = await DoPut(requestUri: METHOD, request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var logginRequest = new RequestDoLoginJson()
        {
            Email = _email,
            Password = _password
        };

        result = await DoPost(requestUri: "login", request: logginRequest);
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        logginRequest.Password = request.NewPassword;

        result = await DoPost(requestUri: "login", request: logginRequest);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Password_Different_Current_Password(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = await DoPut(requestUri: METHOD, request: request, token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager
            .GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = await DoPut(requestUri: METHOD, request: request);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
