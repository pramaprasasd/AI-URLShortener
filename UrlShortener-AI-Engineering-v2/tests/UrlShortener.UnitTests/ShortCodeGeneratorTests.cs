using UrlShortener.Infrastructure;

namespace UrlShortener.UnitTests;

public sealed class ShortCodeGeneratorTests
{
    [Fact]
    public void Generate_ReturnsExpectedLengthAndAlphabet()
    {
        var generator = new ShortCodeGenerator();

        var code = generator.Generate(10);

        Assert.Equal(10, code.Length);
        Assert.All(code, c => Assert.True(char.IsLetterOrDigit(c)));
    }

    [Fact]
    public void Generate_ProducesDifferentValues()
    {
        var generator = new ShortCodeGenerator();

        var values = Enumerable.Range(0, 100)
            .Select(_ => generator.Generate())
            .ToHashSet();

        Assert.True(values.Count > 95);
    }
}