using CommonTestUtilities.Requests;
using RegistroAtendimentoDocente.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WeApi.Test.InlineData;

namespace WeApi.Test.UseCases.Users.UpdateProfile;

public class UpdateProfileUserTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "users/update-profile";
    private readonly string _tokenCoordenador;
    private readonly string _email;
    public UpdateProfileUserTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenCoordenador = webApplicationFactory.User_Coordenador.GetToken();
        _email = webApplicationFactory.User_Admin.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();

        var result = await DoPut(requestUri: METHOD, request: request, token: _tokenCoordenador);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Name_User_Empty(string culture)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(requestUri: METHOD, request: request, token: _tokenCoordenador, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_USER_EMPTY", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Email_Already_Registered(string culture)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = _email;

        var result = await DoPut(requestUri: METHOD, request: request, token: _tokenCoordenador, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_ALREADY_REGISTERED", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Unauthorized_User()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();

        var result = await DoPut(requestUri: METHOD, request: request);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
