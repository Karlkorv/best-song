using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Collections;
namespace best_song.Data;
public class Spotify {

    private readonly IConfiguration _config;
    private SpotifyClient spotifyClient;
    

    public Spotify(IConfiguration config) {
        _config = config;
        var spotifyConfig = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(_config["Spotify:ClientId"],_config["Spotify:ClientSecret"]));
        spotifyClient = new SpotifyClient(spotifyConfig);
    }

    public async Task<List<FullTrack>> GetPlayListTracks(string playlistid) {
        FullPlaylist playlist;
        List<FullTrack> tracks = new List<FullTrack>();
        try
        {
            playlist = await spotifyClient.Playlists.Get(playlistid);
            foreach (PlaylistTrack<IPlayableItem> item in playlist.Tracks.Items)
        {
            if (item.Track is FullTrack track) {
                tracks.Add(track);
            }
        }
        }
        catch (APIException e)
        {
            Console.WriteLine(e.Message);
        }
        return tracks;
    }
}