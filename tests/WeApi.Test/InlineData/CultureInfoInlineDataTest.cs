using System.Collections;

namespace WeApi.Test.InlineData;
public class CultureInfoInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en" };
        yield return new object[] { "pt-BR" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
