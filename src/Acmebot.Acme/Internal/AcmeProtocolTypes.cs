using System.Text.Json.Serialization;

using Acmebot.Acme.Models;

namespace Acmebot.Acme.Internal;

internal sealed record AcmeProtectedHeader
{
    [JsonPropertyName("alg")]
    public required string Algorithm { get; init; }

    [JsonPropertyName("nonce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Nonce { get; init; }

    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("jwk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AcmeJsonWebKey? JsonWebKey { get; init; }

    [JsonPropertyName("kid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? KeyIdentifier { get; init; }
}

internal sealed record AcmeExternalAccountProtectedHeader
{
    [JsonPropertyName("alg")]
    public required string Algorithm { get; init; }

    [JsonPropertyName("kid")]
    public required string KeyIdentifier { get; init; }

    [JsonPropertyName("url")]
    public required string Url { get; init; }
}

internal sealed record AcmeJsonWebKey
{
    [JsonPropertyName("kty")]
    public required string KeyType { get; init; }

    [JsonPropertyName("crv")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Curve { get; init; }

    [JsonPropertyName("x")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? X { get; init; }

    [JsonPropertyName("y")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Y { get; init; }

    [JsonPropertyName("n")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Modulus { get; init; }

    [JsonPropertyName("e")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Exponent { get; init; }

    public string ToThumbprintJson()
    {
        // RFC 7638: JWK members must be in lexicographic order with no whitespace.
        return KeyType switch
        {
            "EC" => $$"""{"crv":"{{Curve}}","kty":"EC","x":"{{X}}","y":"{{Y}}"}""",
            "RSA" => $$"""{"e":"{{Exponent}}","kty":"RSA","n":"{{Modulus}}"}""",
            _ => throw new NotSupportedException("Unsupported JWK type.")
        };
    }
}

internal sealed record AcmeAccountStatusUpdateRequest
{
    [JsonPropertyName("status")]
    public required AcmeAccountStatus Status { get; init; }
}

internal sealed record AcmeAuthorizationStatusUpdateRequest
{
    [JsonPropertyName("status")]
    public required AcmeAuthorizationStatus Status { get; init; }
}

internal sealed record AcmeKeyChangeRequest
{
    [JsonPropertyName("account")]
    public required Uri Account { get; init; }

    [JsonPropertyName("oldKey")]
    public required AcmeJsonWebKey OldKey { get; init; }
}

internal sealed record AcmeEmptyObject;

internal sealed record JsonObjectAccountRequest
{
    [JsonPropertyName("contact")]
    public IReadOnlyList<string> Contact { get; init; } = [];

    [JsonPropertyName("termsOfServiceAgreed")]
    public bool? TermsOfServiceAgreed { get; init; }

    [JsonPropertyName("onlyReturnExisting")]
    public bool? OnlyReturnExisting { get; init; }

    [JsonPropertyName("externalAccountBinding")]
    public AcmeSignedMessage? ExternalAccountBinding { get; init; }
}
