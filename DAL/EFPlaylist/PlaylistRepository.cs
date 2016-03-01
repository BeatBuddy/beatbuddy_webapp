using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;

namespace BB.DAL.EFPlaylist
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly EFDbContext context;
        
        public PlaylistRepository(ContextEnum contextEnum)
        {
            context = new EFDbContext(contextEnum);
        }

        public Comment CreateComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Playlist CreatePlaylist(Playlist playlist)
        {
            playlist = context.Playlists.Add(playlist);
            context.SaveChanges();
            return playlist;
        }

        public Playlist CreatePlaylist(Playlist playlist, Organisation organisation)
        {
            var playlist1 = playlist;
            var organisation1 = context.Organisations.Find(organisation.Id);
            organisation1.Playlists.Add(playlist1);
            context.Playlists.Add(playlist1);
            context.SaveChanges();
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

        public Vote CreateVote(Vote vote, long userId, long trackId)
        {
            var user = context.User.Find(userId);
            vote.User = user;
            vote = context.Votes.Add(vote);
            var playlistTrack = context.PlaylistTracks.Find(trackId);
            playlistTrack.Votes.Add(vote);
            context.SaveChanges();
            return vote;
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
            PlaylistTrack track = context.PlaylistTracks.Single(f => f.Id == playlistTrackId);
            context.PlaylistTracks.Remove(track);
            context.SaveChanges();
        }

        public Track CreateTrack(long playlistId, Track track)
        {
            var playlist = ReadPlaylist(playlistId);
            if (playlist == null) return null;

            var playlistTrack = new PlaylistTrack {Track = track};
            if(playlist.PlaylistTracks == null) playlist.PlaylistTracks = new Collection<PlaylistTrack>();
            else
            {
                if (playlist.PlaylistTracks.Any(f => f.Track.TrackSource.TrackId == track.TrackSource.TrackId)) return null;
            }
            playlist.PlaylistTracks.Add(playlistTrack);

            context.SaveChanges();
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

        public IEnumerable<Comment> ReadChatComments(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> ReadComments(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public Playlist ReadPlaylist(string name)
        {
            return context.Playlists.Single(p => p.Name.Equals(name));
        }

        public Playlist ReadPlaylist(long playlistId)
        {
            return context.Playlists
                .Include(p => p.PlaylistTracks)
                .Include("PlaylistTracks.Track.TrackSource")
                .Include("PlaylistTracks.Votes.User")
                .FirstOrDefault(p => p.Id == playlistId);
        }

        public IEnumerable<Playlist> ReadPlaylists()
        {
            return context.Playlists;
        }

        public IEnumerable<Playlist> ReadPlaylists(Organisation organisation)
        {
            throw new NotImplementedException();
        }

        public PlaylistTrack ReadPlaylistTrack(long playlistTrackId)
        {
            return context.PlaylistTracks.Find(playlistTrackId);
        }

        public IEnumerable<PlaylistTrack> ReadPlaylistTracks(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public Track ReadTrack(long trackId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Track> ReadTracks()
        {
            throw new NotImplementedException();
        }

        public TrackSource ReadTrackSource(long trackSourceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrackSource> ReadTrackSources()
        {
            throw new NotImplementedException();
        }

        public Vote ReadVote(long voteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vote> ReadVotesForPlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vote> ReadVotesUser(User user)
        {
            return context.Votes.Where(v => v.User == user);
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
            if (context.PlaylistTracks.Find(playlistTrack.Id) == null) return null;

            context.PlaylistTracks.Attach(playlistTrack);
            context.Entry(playlistTrack).State = EntityState.Modified;
            context.SaveChanges();

            return playlistTrack;
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
