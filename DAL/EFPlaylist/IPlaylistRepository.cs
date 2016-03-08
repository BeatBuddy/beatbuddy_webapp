using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
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
        IEnumerable<Playlist> ReadPlaylists(long userId);
        Playlist DeletePlaylist(long playlistId);
        IEnumerable<Playlist> ReadPlaylistsForUser(long userId);

        //PlaylistTracks
        PlaylistTrack CreatePlaylistTrack(PlaylistTrack playlistTrack);
        PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack);
        IEnumerable<PlaylistTrack> ReadPlaylistTracks(Playlist playlist);
        PlaylistTrack ReadPlaylistTrack(long playlistTrackId);
        void DeletePlaylistTrack(long playlistTrackId);
        bool SetPlaylistTrackPlayedAtTimestamp(long playlistTrackId);

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
        Vote CreateVote(Vote vote, long userId, long trackId);
        Vote UpdateVote(Vote vote);
        Vote ReadVote(long voteId);
        IEnumerable<Vote> ReadVotesForPlaylist(Playlist playlist);
        IEnumerable<Vote> ReadVotesUser(User user);
        void DeleteVote(long voteId);
        Playlist UpdatePlaylist(Playlist playlist, string email);
        int ReadNumberOfVotesOfUserForPlaylist(long userId, long trackId);
        int ReadMaximumVotesPerUser(long trackId);
        Vote ReadVoteOfUserFromPlaylistTrack(long userId, long trackId);
    }
}
