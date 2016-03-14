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
        Comment UpdateComment(Comment comment);
        IEnumerable<Comment> ReadChatComments(Playlist playlist);
        IEnumerable<Comment> ReadComments(Playlist playlist);
        void DeleteComment(long commentId);
        
        //Playlists
        Playlist CreatePlaylistForOrganisation(string name, string description, string key, int maxVotesPerUser, bool active, string imageUrl, User createdBy, Organisation organisation);
        Playlist CreatePlaylistForUser(string name, string description, string key, int maxVotesPerUser, bool active, string imageUrl, User CreatedBy);
        Playlist UpdatePlaylist(Playlist playlist);
        Playlist ReadPlaylist(long playlistId);
        Playlist ReadPlaylist(string name);
        IEnumerable<Playlist> ReadPlaylists();
        IEnumerable<Playlist> ReadPlaylists(Organisation organisation);
        IEnumerable<Playlist> ReadPlaylists(long userId);
        Playlist DeletePlaylist(long playlistId);
        IEnumerable<Playlist> ReadPlaylistsForUser(long userId);
        bool CheckIfUserCreatedPlaylist(long playlistId, long userId);

        //PlaylistTracks
        PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack);
        IEnumerable<PlaylistTrack> ReadPlaylistTracks(Playlist playlist);
        PlaylistTrack ReadPlaylistTrack(long playlistTrackId);
        void DeletePlaylistTrack(long playlistTrackId);
        bool MarkTrackAsPlayed(long playlistTrackId, long playlistId);
        Playlist UpdatePlaylist(Playlist playlist, string email);
        //Track
        Track AddTrackToPlaylist(long playlistId, Track track);
        Track UpdateTrack(Track track);
        Track ReadTrack(long trackId);
        IEnumerable<Track> ReadTracks();
        void DeleteTrack(long trackId);

        //TrackSource
        TrackSource CreateTrackSource(SourceType sourceType, string url);
        TrackSource UpdateTracksource(TrackSource trackSource);
        TrackSource ReadTrackSource(long trackSourceId);
        IEnumerable<TrackSource> ReadTrackSources();
        void DeleteTrackSource(long trackSourceId);

        //Vote
        Vote CreateVote(int score, long userId, long trackId);
        Vote UpdateVote(Vote vote);
        Vote ReadVote(long voteId);
        IEnumerable<Vote> ReadVotesForPlaylist(Playlist playlist);
        IEnumerable<Vote> ReadVotesForUser(User user);
        void DeleteVote(long playlistTrackId, long userId);



    }
}
