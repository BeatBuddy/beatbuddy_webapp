using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using System.Collections.Generic;

namespace BB.DAL.EFPlaylist
{
    public interface IPlaylistRepository
    {
        //Comments
        Comment CreateComment(Comment comment);
        Comment UpdateComment(Comment comment);
        IEnumerable<Comment> ReadChatComments(Playlist playlist);
        IEnumerable<Comment> ReadComments(Playlist playlist);
        void DeleteComment(long commentId);

        //Playlists
        Playlist CreatePlaylist(Playlist playlist);
        Playlist CreatePlaylist(Playlist playlist, Organisation organisation);
        Playlist UpdatePlaylist(Playlist playlist);
        Playlist ReadPlaylist(long playlistId);
        Playlist ReadPlaylist(string name);
        IEnumerable<Playlist> ReadPlaylists();
        IEnumerable<Playlist> ReadPlaylists(Organisation organisation);
        void DeletePlaylist(long playlistId);

        //PlaylistTracks
        PlaylistTrack CreatePlaylistTrack(PlaylistTrack playlistTrack);
        PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack);
        IEnumerable<PlaylistTrack> ReadPlaylistTracks(Playlist playlist);
        PlaylistTrack ReadPlaylistTrack(long playlistTrackId);
        void DeletePlaylistTrack(long playlistTrackId);

        //Track
        Track CreateTrack(long playlistId, Track track);
        Track UpdateTrack(Track track);
        Track ReadTrack(long trackId);
        IEnumerable<Track> ReadTracks();
        void DeleteTrack(long trackId);

        //TrackSource
        TrackSource CreateTrackSource(TrackSource trackSource);
        TrackSource UpdateTracksource(TrackSource trackSource);
        TrackSource ReadTrackSource(long trackSourceId);
        IEnumerable<TrackSource> ReadTrackSources();
        void DeleteTrackSource(long trackSourceId);

        //Vote
        Vote CreateVote(Vote vote);
        Vote UpdateVote(Vote vote);
        Vote ReadVote(long voteId);
        IEnumerable<Vote> ReadVotesForPlaylist(Playlist playlist);
        void DeleteVote(long voteId);
    }
}
