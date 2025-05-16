using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TravelJournal.Domain.Interfaces;

namespace TravelJournal.Infrastructure.Security;

public class JwtSettings
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpiresMinutes { get; set; }
}

public class TokenGenerator : ITokenGenerator
{
    private readonly byte[] _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _lifetimeMinutes;

    public TokenGenerator(IOptions<JwtSettings> options)
    {
        var settings = options?.Value
                       ?? throw new ArgumentNullException(nameof(options), "JWT settings not provided");

        if (string.IsNullOrWhiteSpace(settings.Key))
            throw new ArgumentException("JWT Key is not configured", nameof(settings.Key));

        _key = Encoding.UTF8.GetBytes(settings.Key);
        _issuer = settings.Issuer;
        _audience = settings.Audience;
        _lifetimeMinutes = settings.ExpiresMinutes;
    }

    public string GenerateToken(int id, string username)
    {
        var header = new Dictionary<string, object>
        {
            ["alg"] = "HS256",
            ["typ"] = "JWT"
        };

        var now = DateTimeOffset.UtcNow;
        var payload = new Dictionary<string, object>
        {
            ["sub"] = username,
            ["id"] = id,
            ["iss"] = _issuer,
            ["aud"] = _audience,
            ["iat"] = now.ToUnixTimeSeconds(),
            ["exp"] = now.AddMinutes(_lifetimeMinutes).ToUnixTimeSeconds(),
            ["jti"] = Guid.NewGuid().ToString()
        };

        string headerJson = JsonSerializer.Serialize(header);
        string payloadJson = JsonSerializer.Serialize(payload);

        string headerBase64 = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
        string payloadBase64 = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

        var unsignedToken = $"{headerBase64}.{payloadBase64}";
        var signature = Base64UrlEncode(Sign(unsignedToken));

        return $"{unsignedToken}.{signature}";
    }

    private byte[] Sign(string data)
    {
        using var hmac = new HMACSHA256(_key);
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    private static string Base64UrlEncode(byte[] input)
        => Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
}