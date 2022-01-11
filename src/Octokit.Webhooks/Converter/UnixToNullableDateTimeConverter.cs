using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Octokit.Webhooks.Converter;

public class UnixToNullableDateTimeConverter : JsonConverter<DateTimeOffset?>
{
    public override bool HandleNull => true;
    public bool? IsFormatInSeconds { get; set; } = null;

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var time))
        {
            // if 'IsFormatInSeconds' is unspecified, then deduce the correct type based on whether it can be represented in the allowed .net DateTime range
            if (IsFormatInSeconds == true || IsFormatInSeconds == null && time > _unixMinSeconds && time < _unixMaxSeconds)
                return DateTimeOffset.FromUnixTimeSeconds(time).UtcDateTime;
            return DateTimeOffset.FromUnixTimeMilliseconds(time).UtcDateTime;
        }

        if (reader.TokenType == JsonTokenType.String && reader.TryGetDateTimeOffset(out var dateTimeOffset))
            return dateTimeOffset;

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options) => throw new NotSupportedException();

    private static readonly long _unixMinSeconds = DateTimeOffset.MinValue.ToUnixTimeSeconds() - DateTimeOffset.UnixEpoch.ToUnixTimeSeconds(); // -62_135_596_800
    private static readonly long _unixMaxSeconds = DateTimeOffset.MaxValue.ToUnixTimeSeconds() - DateTimeOffset.UnixEpoch.ToUnixTimeSeconds(); // 253_402_300_799
}