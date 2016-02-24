using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;

namespace BB.DAL.EFPlaylist
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private EFDbContext ctx;
        
        public PlaylistRepository(ContextEnum contextEnum)
        {
            ctx = new EFDbContext(contextEnum);
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

        public Playlist CreatePlaylist(Playlist playlist, Organisation organisation)
        {
            var playlist1 = playlist;
            var organisation1 = ctx.Organisations.Find(organisation.Id);
            organisation1.Playlists.Add(playlist1);
            ctx.Playlists.Add(playlist1);
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
            PlaylistTrack track = ctx.PlaylistTracks.Single(f => f.Id == playlistTrackId);
            ctx.PlaylistTracks.Remove(track);
            ctx.SaveChanges();
        }

        public Track CreateTrack(long playlistId, Track track)
        {
            var playlist = ctx.Playlists.Find(playlistId);
            if (playlist == null) return null;

            var playlistTrack = new PlaylistTrack {Track = track};
            if(playlist.PlaylistTracks == null) playlist.PlaylistTracks = new Collection<PlaylistTrack>();
            playlist.PlaylistTracks.Add(playlistTrack);

            ctx.SaveChanges();
            return playlistTrack.Track;
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
            return ctx.Playlists.Single(p => p.Name.Equals(name));
        }

        public Playlist ReadPlaylist(long playlistId)
        {
            return ctx.Playlists
                .Include(p => p.PlaylistTracks)
                .Include("PlaylistTracks.Track.TrackSource")
                .First(p => p.Id == playlistId);
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
