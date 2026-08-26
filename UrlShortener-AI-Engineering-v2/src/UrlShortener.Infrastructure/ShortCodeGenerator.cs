using System.Security.Cryptography;
using UrlShortener.Application.Contracts;

namespace UrlShortener.Infrastructure;

public sealed class ShortCodeGenerator : IShortCodeGenerator
{
    private const string Alphabet =
        "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public string Generate(int length = 7)
    {
        if (length is < 6 or > 20)
            throw new ArgumentOutOfRangeException(nameof(length));

        Span<byte> bytes = stackalloc byte[length];
        RandomNumberGenerator.Fill(bytes);

        Span<char> result = stackalloc char[length];

        for (var i = 0; i < length; i++)
            result[i] = Alphabet[bytes[i] % Alphabet.Length];

        return new string(result);
    }
}