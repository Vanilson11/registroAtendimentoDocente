using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Users.Register;
public class RegisterUserTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users";
    private readonly string _email;

    public RegisterUserTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.User_Others.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request);

        result.StatusCode.ShouldBe(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Name_Empty(string cultureInfo)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPost(requestUri: METHOD, request: request, culture: cultureInfo);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_USER_EMPTY", new CultureInfo(cultureInfo));

        errors.Single().ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Email_All_Registered(string cultureInfo)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = _email;

        var result = await DoPost(requestUri: METHOD, request: request, culture: cultureInfo);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select (error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_ALREADY_REGISTERED", new CultureInfo(cultureInfo));

        errors.Single().ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }
}
