using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Collections;
using System.Text.RegularExpressions;
using System;
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
            if (item.Track is FullTrack track) {
                tracks.Add(track);
            }
        }
        return tracks;
    }
}