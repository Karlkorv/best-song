
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

/// <summary>
/// Den här klassen används bara för att få preview_url från dem tracks som webscrapas
/// Är temporär och bör bytas ut snaras
/// </summary>
public class Track
{
    [JsonProperty("preview_url")]
    public string PreviewUrl { get; set; } = default!;
}
internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters = {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}