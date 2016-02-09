using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.DAL.EFPlaylist
{
    public interface IPlaylistRepository
    {
        //Comments
        Comment CreateComment(Comment comment);
        Comment UpdateComment(Comment comment);
        List<Comment> ReadChatComments(Playlist playlist);
        List<Comment> ReadComments(Playlist playlist);
        void DeleteComment(long commentId);

        //Playlists
        Playlist CreatePlaylist(Playlist playlist);
        Playlist UpdatePlaylist(Playlist playlist);
        Playlist ReadPlaylist(long playlistId);
        Playlist ReadPlaylist(string name);
        List<Playlist> ReadPlaylists();
        List<Playlist> ReadPlaylists(Organisation organisation);
        void DeletePlaylist(long playlistId);

        //PlaylistTracks
        PlaylistTrack CreatePlaylistTrack(PlaylistTrack playlistTrack);
        PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack);
        List<PlaylistTrack> ReadPlaylistTracks(Playlist playlist);
        PlaylistTrack ReadPlaylistTrack(long playlistTrackId);
        void DeletePlaylistTrack(long playlistTrackId);

        //Track
        Track CreateTrack(Track track);
        Track UpdateTrack(Track track);
        Track ReadTrack(long trackId);
        List<Track> ReadTracks();
        void DeleteTrack(long trackId);

        //TrackSource
        TrackSource CreateTrackSource(TrackSource trackSource);
        TrackSource UpdateTracksource(TrackSource trackSource);
        TrackSource ReadTrackSource(long trackSourceId);
        List<TrackSource> ReadTrackSources();
        void DeleteTrackSource(long trackSourceId);

        //Vote
        Vote CreateVote(Vote vote);
        Vote UpdateVote(Vote vote);
        Vote ReadVote(long voteId);
        List<Vote> ReadVotesForPlaylist(Playlist playlist);
        void DeleteVote(long voteId);
    }
}
