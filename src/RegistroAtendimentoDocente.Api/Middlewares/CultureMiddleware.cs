using System.Globalization;

namespace RegistroAtendimentoDocente.Api.Middlewares;

public class CultureMiddleware
{
    private RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var languagesSupported = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        var requestedLanguage = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        var cultureInfo = new CultureInfo("en");

        if(string.IsNullOrWhiteSpace(requestedLanguage) == false &&
            languagesSupported.Exists(language => language.Name.Equals(requestedLanguage)))
        {
            cultureInfo = new CultureInfo(requestedLanguage);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
