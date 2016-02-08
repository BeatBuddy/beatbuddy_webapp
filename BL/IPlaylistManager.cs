using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL
{
    public interface IPlaylistManager
    {
        //Comments
        Comment CreateComment(string text, User user, Playlist playlist);
        Comment UpdateComment(Comment comment);
        List<Comment> ReadComments(Playlist playlist);
        void DeleteComment(long commentId);
        
        //Playlists
        Playlist CreatePlaylist(string name, int maxVotesPerUser, bool active, string imageUrl, Organisation organisation);
        Playlist UpdatePlaylist(Playlist playlist);
        Playlist readPlaylist(long playlistId);
        Playlist readPlaylist(string name);
        List<Playlist> readPlaylists();
        List<Playlist> readPlaylists(Organisation organisation);
        void DeletePlaylist(long playlistId);

        //PlaylistTracks
        PlaylistTrack CreatePlaylistTrack(Track track, Playlist playlist);
        PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack);
        List<PlaylistTrack> ReadPlaylistTracks(Playlist playlist);
        PlaylistTrack ReadPlaylistTrack(long playlistTrackId);
        void DeletePlaylistTrack(long playlistTrackId);

        //Track
        Track CreateTrack(string artist, string title, TrackSource trackSource, string coverArtUrl);
        Track UpdateTrack(Track track);
        Track ReadTrack(long trackId);
        List<Track> ReadTracks();
        void DeleteTrack(long trackId);

        //TrackSource
        TrackSource CreateTrackSource(SourceType sourceType, string url);
    }
}
