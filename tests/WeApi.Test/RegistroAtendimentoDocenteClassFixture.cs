using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WeApi.Test;
public class RegistroAtendimentoDocenteClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public RegistroAtendimentoDocenteClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    public async Task<HttpResponseMessage> DoPost(string requestUri, object request, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        SetCulture(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    private void AuthorizeRequest(string token)
    {
        if(string.IsNullOrWhiteSpace(token) == false)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private void SetCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}
