using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Text;
using HtmlAgilityPack;
namespace best_song.Data;
public class Spotify {

    private readonly IConfiguration _config;
    private SpotifyClient _spotifyClient;
    

    public Spotify(IConfiguration config) 
    {
        _config = config;
        var spotifyConfig = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(_config["Spotify:ClientId"],_config["Spotify:ClientSecret"]));
        _spotifyClient = new SpotifyClient(spotifyConfig);
    }
    /// <summary>
    /// Returns a list of all tracks in a playlist.
    /// </summary>
    /// <param name="link">The link to the playlist</param>
    /// <returns>
    /// A list of type FullTrack of all the tracks in the playlist. 
    /// The list is empty if he playlist link is invalid.
    /// </returns>
    public async Task<List<FullTrack>> GetPlayList(string link) 
    {
        Regex rx = new Regex(@"(?<=playlist/)(.*?)(?=\?)", RegexOptions.Compiled);
        var match = rx.Matches(link);
        if (match.Count != 1) 
        {
           return new List<FullTrack>(0);
        }
        return await GetPlayListTracksFromId(match[0].Value);
    }

    /// <summary>
    /// Takes a playlist id and returns a list of all FullTracks
    /// </summary>
    /// <param name="playlistid"></param>
    /// <exception cref="ApiException"></throws>
    /// <returns>A list of FullTrack</returns>
    public async Task<List<FullTrack>> GetPlayListTracksFromId(string playlistid) 
    {
        FullPlaylist playlist = await _spotifyClient.Playlists.Get(playlistid);
        List<FullTrack> tracks = new List<FullTrack>();
        foreach (PlaylistTrack<IPlayableItem> item in playlist.Tracks.Items)
        {
            if (item.Track is FullTrack track) 
            {
                if (track.PreviewUrl == null)
               {
                   track.PreviewUrl = GetPreviewFromId(track.Id);
               }
                tracks.Add(track);
            }
        }
        return tracks;
    }
    /// <summary>
    /// Ful kod? Ja
    /// Funkar? Ja
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetPreviewFromId(string id) 
    {
        StringBuilder embedUrl = new StringBuilder("https://open.spotify.com/embed?uri=spotify:track:");
        embedUrl.Append(id);
        var body = GetDocument(embedUrl.ToString())
                .DocumentNode.SelectSingleNode("//body/script[@id='resource']")
                .InnerHtml;

        Regex rg = new Regex(@"(?<=mp3-preview%2F)(.*?)(?=%22)");
        StringBuilder link = new StringBuilder("https://p.scdn.co/mp3-preview/");
        var linkExt = rg.Matches(body)[0].Value;
        for (int i = 0; i < linkExt.Length; i++)
        {
            if (linkExt[i] == '%')
            {
                StringBuilder hex = new StringBuilder();
                hex.Append(linkExt[i+1]).Append(linkExt[i+2]);
                string value = Char.ConvertFromUtf32(Convert.ToInt32(hex.ToString(), 16));
                link.Append(value);
                i=i+2;
            }
            link.Append(linkExt[i]);
        }
        return link.ToString();
    }
    private static HtmlDocument GetDocument(string url)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);
        return doc;
    }

    public async Task<string> GetTrack(string id) 
    {

        var track = await _spotifyClient.Tracks.Get(id);
        Console.WriteLine(track.PreviewUrl);
        return track.PreviewUrl;
    }
}