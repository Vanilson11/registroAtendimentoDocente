using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WeApi.Test.UseCases.Atendimentos.Register;
public class RegisterAtendimentoTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "atendimento";
    private readonly string _token;
    public RegisterAtendimentoTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Coordenador.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterAtendimentoJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.Created);

        using var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("docente").GetString().ShouldBe(request.Docente);
        response.RootElement.GetProperty("assunto").GetString().ShouldBe(request.Assunto);
    }
}
