
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

/// <summary>
/// A class that represents a spotify track
/// Similar to SpotifyAPI.Web.Fulltrack
/// </summary>
public class Track
{
    [JsonProperty("album")]
    public Album Album { get; set; } = default!;

    [JsonProperty("artists")]
    public Artist[] Artists { get; set; } = default!;

    [JsonProperty("disc_number")]
    public long DiscNumber { get; set; } = default!;

    [JsonProperty("duration_ms")]
    public long DurationMs { get; set; } = default!;

    [JsonProperty("explicit")]
    public bool Explicit { get; set; } = default!;

    [JsonProperty("external_ids")]
    public ExternalIds ExternalIds { get; set; } = default!;

    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; } = default!;

    [JsonProperty("href")]
    public Uri Href { get; set; } = default!;

    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("is_local")]
    public bool IsLocal { get; set; } = default!;

    [JsonProperty("is_playable")]
    public bool IsPlayable { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("popularity")]
    public long Popularity { get; set; } = default!;

    [JsonProperty("preview_url")]
    public string PreviewUrl { get; set; } = default!;

    [JsonProperty("track_number")]
    public long TrackNumber { get; set; } = default!;

    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("uri")]
    public string Uri { get; set; } = default!;

    [JsonProperty("dominantColor")]
    public string DominantColor { get; set; } = default!;
}

public partial class Album
{
    [JsonProperty("album_type")]
    public string AlbumType { get; set; } = default!;

    [JsonProperty("artists")]
    public Artist[] Artists { get; set; } = default!;

    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; } = default!;

    [JsonProperty("href")]
    public Uri Href { get; set; } = default!;

    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("images")]
    public Image[] Images { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("release_date")]
    public DateTimeOffset ReleaseDate { get; set; } = default!;

    [JsonProperty("release_date_precision")]
    public string ReleaseDatePrecision { get; set; } = default!;

    [JsonProperty("total_tracks")]
    public long TotalTracks { get; set; } = default!;

    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("uri")]
    public string Uri { get; set; } = default!;
}

public partial class Artist
{
    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; } = default!;

    [JsonProperty("href")]
    public Uri Href { get; set; } = default!;

    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("uri")]
    public string Uri { get; set; } = default!;
}

public partial class ExternalUrls
{
    [JsonProperty("spotify")]
    public Uri Spotify { get; set; } = default!;
}

public partial class Image
{
    [JsonProperty("height")]
    public long Height { get; set; } = default!;

    [JsonProperty("url")]
    public Uri Url { get; set; } = default!;

    [JsonProperty("width")]
    public long Width { get; set; } = default!;
}

public partial class ExternalIds
{
    [JsonProperty("isrc")]
    public string Isrc { get; set; } = default!;
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