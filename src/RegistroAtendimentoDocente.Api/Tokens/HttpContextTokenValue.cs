using RegistroAtendimentoDocente.Domain.Security.Tokens;

namespace RegistroAtendimentoDocente.Api.Tokens;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }
    public string GetTokenOnRequest()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization["Bearer".Length..].Trim();
    }
}
