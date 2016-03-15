using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using System.Collections.Generic;

namespace BB.DAL.EFPlaylist
{
    public interface IPlaylistRepository
    {
        //Comments
        Comment CreateComment(long playlistId, Comment comment);
        IEnumerable<Comment> ReadComments(Playlist playlist);

        //Playlists
        Playlist CreatePlaylist(Playlist playlist);
        Playlist CreatePlaylist(Playlist playlist, long organisationid);
        Playlist UpdatePlaylist(Playlist playlist);
        Playlist ReadPlaylist(long playlistId);
        Playlist ReadPlaylist(string name);
        IEnumerable<Playlist> ReadPlaylists();
        IEnumerable<Playlist> ReadPlaylists(long userId);
        Playlist DeletePlaylist(long playlistId);
        IEnumerable<Playlist> ReadPlaylistsForUser(long userId);

            //PlaylistTracks
        PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack);
        PlaylistTrack ReadPlaylistTrack(long playlistTrackId);
        void DeletePlaylistTrack(long playlistTrackId);
        bool MarkTrackAsPlayed(long playlistTrackId, long playlistId);

        //Track
        Track CreateTrack(long playlistId, Track track);
        IEnumerable<Track> ReadTracks();
        Track ReadTrack(long trackId);

        //TrackSource

        //Vote
        Vote CreateVote(Vote vote, long userId, long trackId);
        Vote UpdateVote(Vote vote);
        IEnumerable<Vote> ReadVotesUser(User user);
        void DeleteVote(long voteId);
        void DeleteVote(Vote vote);
        Playlist UpdatePlaylist(Playlist playlist, string email);
        int ReadNumberOfVotesOfUserForPlaylist(long userId, long trackId);
        int ReadMaximumVotesPerUser(long trackId);
        Vote ReadVoteOfUserFromPlaylistTrack(long userId, long trackId);
        
    }
}
