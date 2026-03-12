using Shouldly;
using System.Net;
using System.Net.Mime;

namespace WeApi.Test.UseCases.Atendimentos.Reports;

public class GetReportsTests : RegistroAtendimentoDocenteClassFixture
{
    private const string METHOD = "reports";
    private readonly string _tokenAdmin;
    private readonly string _tokenCoordenador;
    private readonly DateTime _data;
    public GetReportsTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _tokenCoordenador = webApplicationFactory.User_Coordenador.GetToken();
        _data = webApplicationFactory.Atendimento_Coordenador.GetData();
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_data:yyyy-MM}", token: _tokenCoordenador);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_data:yyyy-MM}", token: _tokenCoordenador);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_data:yyyy-MM}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_data:yyyy-MM}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
