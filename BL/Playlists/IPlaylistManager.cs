using System;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using System.Collections.Generic;

namespace BB.BL
{
    public interface IPlaylistManager
    {
        //Comments
        Comment CreateComment(long playlistId, string text, string userEmail);
        IEnumerable<Comment> ReadComments(Playlist playlist);
        
        //Playlists
        Playlist CreatePlaylistForOrganisation(string name, string description, string key, int maxVotesPerUser, bool active, string imageUrl, User createdBy, long organisationId);
        Playlist CreatePlaylistForUser(string name, string description, string key, int maxVotesPerUser, bool active, string imageUrl, User CreatedBy);
        Playlist UpdatePlaylist(Playlist playlist);
        Playlist ReadPlaylist(long playlistId);
        Playlist ReadPlaylist(string name);
        IEnumerable<Playlist> ReadPlaylists();
        IEnumerable<Playlist> ReadPlaylists(long userId);
        Playlist DeletePlaylist(long playlistId);
        IEnumerable<Playlist> ReadPlaylistsForUser(long userId);
        bool CheckIfUserCreatedPlaylist(long playlistId, long userId);

        //PlaylistTracks
        PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack);
        PlaylistTrack ReadPlaylistTrack(long playlistTrackId);
        void DeletePlaylistTrack(long playlistTrackId);
        bool MarkTrackAsPlayed(long playlistTrackId, long playlistId);
        Playlist UpdatePlaylist(Playlist playlist, string email);

        //Track
        Track AddTrackToPlaylist(long playlistId, Track track);
        IEnumerable<Track> ReadTracks();

        //TrackSource

        //Vote
        Vote CreateVote(int score, long userId, long trackId);
        Vote UpdateVote(Vote vote);
        IEnumerable<Vote> ReadVotesForUser(User user);
        void DeleteVote(long playlistTrackId, long userId);



    }
}
