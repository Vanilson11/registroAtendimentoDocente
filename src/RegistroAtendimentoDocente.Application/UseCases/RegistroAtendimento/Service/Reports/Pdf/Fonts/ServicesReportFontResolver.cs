using PdfSharp.Fonts;
using System.Reflection;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Pdf.Fonts;
public class ServicesReportFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName);

        if (stream is null)
            stream = ReadFontFile(FontsHelper.DEFAULT_FONT);

        var length = (int)stream!.Length;

        var data = new byte[length];

        stream.Read(buffer: data, offset: 0, count: length);

        return data;
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream($"RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Pdf.Fonts.{faceName}.ttf");
    }
}
