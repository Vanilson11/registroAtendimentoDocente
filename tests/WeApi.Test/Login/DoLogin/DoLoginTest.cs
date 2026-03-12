using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.Login.DoLogin;
public class DoLoginTest : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "login";
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.User_Others.GetEmail();
        _password = webApplicationFactory.User_Others.GetPassword();
        _name = webApplicationFactory.User_Others.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestDoLoginJson()
        {
            Email = _email,
            Password = _password
        };

        var result = await DoPost(requestUri: METHOD, request: request);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Invalid_Login(string cultureInfo)
    {
        var request = RequestDoLoginJsonBuilder.Build();
        request.Email = _email;

        var result = await DoPost(requestUri: METHOD, request: request, culture: cultureInfo);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("INVALID_LOGIN", new CultureInfo(cultureInfo));

        errors.Single().ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }
}
