using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;

namespace BB.DAL.EFPlaylist
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private EFDbContext ctx;
        
        public PlaylistRepository()
        {
            ctx = new EFDbContext();
        }

        public Comment CreateComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Playlist CreatePlaylist(Playlist playlist)
        {
            playlist = ctx.Playlists.Add(playlist);
            ctx.SaveChanges();
            return playlist;
        }

        public PlaylistTrack CreatePlaylistTrack(PlaylistTrack playlistTrack)
        {
            throw new NotImplementedException();
        }

        public Track CreateTrack(Track track)
        {
            throw new NotImplementedException();
        }

        public TrackSource CreateTrackSource(TrackSource trackSource)
        {
            throw new NotImplementedException();
        }

        public Vote CreateVote(Vote vote)
        {
            throw new NotImplementedException();
        }

        public void DeleteComment(long commentId)
        {
            throw new NotImplementedException();
        }

        public void DeletePlaylist(long playlistId)
        {
            throw new NotImplementedException();
        }

        public void DeletePlaylistTrack(long playlistTrackId)
        {
            throw new NotImplementedException();
        }

        public void DeleteTrack(long trackId)
        {
            throw new NotImplementedException();
        }

        public void DeleteTrackSource(long trackSourceId)
        {
            throw new NotImplementedException();
        }

        public void DeleteVote(long voteId)
        {
            throw new NotImplementedException();
        }

        public List<Comment> ReadChatComments(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public List<Comment> ReadComments(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public Playlist ReadPlaylist(string name)
        {
            throw new NotImplementedException();
        }

        public Playlist ReadPlaylist(long playlistId)
        {
            throw new NotImplementedException();
        }

        public List<Playlist> ReadPlaylists()
        {
            return ctx.Playlists.ToList();
        }

        public List<Playlist> ReadPlaylists(Organisation organisation)
        {
            throw new NotImplementedException();
        }

        public PlaylistTrack ReadPlaylistTrack(long playlistTrackId)
        {
            throw new NotImplementedException();
        }

        public List<PlaylistTrack> ReadPlaylistTracks(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public Track ReadTrack(long trackId)
        {
            throw new NotImplementedException();
        }

        public List<Track> ReadTracks()
        {
            throw new NotImplementedException();
        }

        public TrackSource ReadTrackSource(long trackSourceId)
        {
            throw new NotImplementedException();
        }

        public List<TrackSource> ReadTrackSources()
        {
            throw new NotImplementedException();
        }

        public Vote ReadVote(long voteId)
        {
            throw new NotImplementedException();
        }

        public List<Vote> ReadVotesForPlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public Comment UpdateComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Playlist UpdatePlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack)
        {
            throw new NotImplementedException();
        }

        public Track UpdateTrack(Track track)
        {
            throw new NotImplementedException();
        }

        public TrackSource UpdateTracksource(TrackSource trackSource)
        {
            throw new NotImplementedException();
        }

        public Vote UpdateVote(Vote vote)
        {
            throw new NotImplementedException();
        }
    }
}
