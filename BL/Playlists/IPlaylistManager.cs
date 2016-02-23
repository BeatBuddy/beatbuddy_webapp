﻿using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using System.Collections.Generic;

namespace BB.BL
{
    public interface IPlaylistManager
    {
        //Comments
        Comment CreateComment(string text, User user);
        Comment UpdateComment(Comment comment);
        List<Comment> ReadChatComments(Playlist playlist);
        List<Comment> ReadComments(Playlist playlist);
        void DeleteComment(long commentId);
        
        //Playlists
        Playlist CreatePlaylistForOrganisation(string name, int maxVotesPerUser, bool active, string imageUrl, User playlistMaster, User createdBy, Organisation organisation);
        Playlist CreatePlaylistForUser(string name, string description, string key, int maxVotesPerUser, bool active, string imageUrl, User playlistMaster, User CreatedBy);
        Playlist UpdatePlaylist(Playlist playlist);
        Playlist ReadPlaylist(long playlistId);
        Playlist ReadPlaylist(string name);
        List<Playlist> ReadPlaylists();
        List<Playlist> ReadPlaylists(Organisation organisation);
        void DeletePlaylist(long playlistId);

        //PlaylistTracks
        PlaylistTrack CreatePlaylistTrack(Track track);
        PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack);
        List<PlaylistTrack> ReadPlaylistTracks(Playlist playlist);
        PlaylistTrack ReadPlaylistTrack(long playlistTrackId);
        void DeletePlaylistTrack(long playlistTrackId);

        //Track
        Track AddTrackToPlaylist(long playlistId, string artist, string title, TrackSource trackSource, string coverArtUrl);
        Track UpdateTrack(Track track);
        Track ReadTrack(long trackId);
        List<Track> ReadTracks();
        void DeleteTrack(long trackId);

        //TrackSource
        TrackSource CreateTrackSource(SourceType sourceType, string url);
        TrackSource UpdateTracksource(TrackSource trackSource);
        TrackSource ReadTrackSource(long trackSourceId);
        List<TrackSource> ReadTrackSources();
        void DeleteTrackSource(long trackSourceId);

        //Vote
        Vote CreateVote(int score, User user);
        Vote UpdateVote(Vote vote);
        Vote ReadVote(long voteId);
        List<Vote> ReadVotesForPlaylist(Playlist playlist);
        void DeleteVote(long voteId);
    }
}
