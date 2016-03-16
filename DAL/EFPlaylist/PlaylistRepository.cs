using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;

namespace BB.DAL.EFPlaylist
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly EFDbContext context;
        
        public PlaylistRepository(EFDbContext context)
        {
            this.context = context;
        }

        public Comment CreateComment(long playlistId, Comment comment)
        {
            var playlist = context.Playlists
                .Include(p => p.Comments)
                .First(p => p.Id == playlistId);

            var user = context.User.Single(u => u.Id == comment.User.Id);
            comment.User = user;

            var createdComment = context.Comments.Add(comment);
            playlist.Comments.Add(createdComment);

            context.SaveChanges();
            return comment;
        }

        public Playlist CreatePlaylist(Playlist playlist)
        {
            playlist = context.Playlists.Add(playlist);
            context.SaveChanges();
            return playlist;
        }

        public Playlist CreatePlaylist(Playlist playlist, long organisationId)
        {
            var organisation = context.Organisations.Find(organisationId);
            organisation.Playlists.Add(playlist);
            context.SaveChanges();
            return playlist;
        }

        public IEnumerable<Playlist> ReadPlaylistsForUser(long userId)
        {
           return context.Playlists.ToList().FindAll(p => p.CreatedById == userId);
        }

        public IEnumerable<Playlist> ReadPlaylists(long userId)
        {
            return context.Playlists.Where(p => p.CreatedById == userId);
        }

        public Playlist DeletePlaylist(long playlistId)
        {
            var playlist = ReadPlaylist(playlistId);

            playlist = context.Playlists.Remove(playlist);
            context.SaveChanges();
            return playlist ;
        }

        public void DeletePlaylistTrack(long playlistTrackId)
        {
            PlaylistTrack track = context.PlaylistTracks.Single(f => f.Id == playlistTrackId);
            context.PlaylistTracks.Remove(track);
            context.SaveChanges();
        }

        public bool MarkTrackAsPlayed(long id, long playlistId)
        {
            var playlist = context.Playlists.Include(p=>p.PlaylistTracks).Single(p => p.Id == playlistId);
            var track = playlist.PlaylistTracks.Single(t => t.Id == id);
            if (track == null) return false;
            track.PlayedAt = DateTime.Now;
          
            context.Entry(track).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public Track CreateTrack(long playlistId, Track track)
        {
            var playlist = ReadPlaylist(playlistId);
            if (playlist == null) return null;

            var playlistTrack = new PlaylistTrack {Track = track};
            if(playlist.PlaylistTracks == null) playlist.PlaylistTracks = new Collection<PlaylistTrack>();
            
            playlist.PlaylistTracks.Add(playlistTrack);
           
            context.SaveChanges();
            
            return playlistTrack.Track;
        }

        public void DeleteVote(long voteId)
        {
            var vote = context.Votes.Find(voteId);
            context.Votes.Remove(vote);
            context.SaveChanges();
        }

        public void DeleteVote(Vote vote)
        {
            context.Votes.Remove(vote);
            context.SaveChanges();
        }

        public Playlist UpdatePlaylist(Playlist playlist, string email)
        {
            var pl = context.Playlists.ToList().Single(p => p.Id == playlist.Id);
            if (email == null)
            {
                pl.PlaylistMasterId = null;
            }
            else
            {
                var user = context.User.Single(u => u.Email == email);
                pl.PlaylistMasterId = user.Id;
            }
            context.Entry(pl).State = EntityState.Modified;
            context.SaveChanges();

            return playlist;

        }

        public IEnumerable<Comment> ReadComments(Playlist playlist)
        {
            var pl = context.Playlists
                .Include(p => p.Comments)
                .Include("Comments.User")
                .First(p => p.Id == playlist.Id);
            return pl.Comments;
        }

        public Playlist ReadPlaylist(string name)
        {
            return context.Playlists.SingleOrDefault(p => p.Name.Equals(name));
        }

        public Playlist ReadPlaylist(long playlistId)
        {
            var playlist = context.Playlists
                .Include(p => p.PlaylistTracks)
                .Include(p => p.PlaylistTracks.Select(pt => pt.Track))
                .Include(p => p.PlaylistTracks.Select(pt => pt.Track.TrackSource))
                .Include(p => p.PlaylistTracks.Select(pt => pt.Votes))
                .Include(p => p.PlaylistTracks.Select(pt => pt.Votes.Select(v => v.User)))
                .ToList()
                .SingleOrDefault(p => p.Id == playlistId);

            if (playlist == null) return null;

            var tracks = context.PlaylistTracks.Where(p => p.PlaylistId == playlistId).ToList();
            playlist.PlaylistTracks = tracks;

            return playlist;
        }

        public IEnumerable<Playlist> ReadPlaylists()
        {
            return context.Playlists.ToList();
        }

        public PlaylistTrack ReadPlaylistTrack(long playlistTrackId)
        {
            return context.PlaylistTracks
                .Include(p => p.Votes)
                .Include(p => p.Votes.Select(v => v.User))
                .Include(p => p.Track)
                .Include(p => p.Track.TrackSource)
                .FirstOrDefault(p => p.Id == playlistTrackId);
        }

        public IEnumerable<Track> ReadTracks()
        {
            return context.Tracks;
        }

        public Track ReadTrack(long trackId)
        {
            return context.Tracks
                .Include(t => t.TrackSource)
                .Single(t => t.Id == trackId);
        }

        public IEnumerable<Vote> ReadVotesUser(User user)
        {
            return context.Votes.Where(v => v.User == user);
        }


        public Vote CreateVote(Vote vote, long userId, long trackId)
        {
            var user = context.User.Find(userId);
            vote.User = user;
            var playlistTrack = context.PlaylistTracks.Find(trackId);
            vote = context.Votes.Add(vote);
            if (playlistTrack.Votes == null) playlistTrack.Votes = new Collection<Vote>();
            playlistTrack.Votes.Add(vote);
            context.SaveChanges();
            return vote;
        }

        public int ReadMaximumVotesPerUser(long trackId)
        {
            var playlist = context.Playlists.FirstOrDefault(p => p.PlaylistTracks.Any(t => t.Id == trackId));
            return playlist?.MaximumVotesPerUser ?? 0;
        }

        public int ReadNumberOfVotesOfUserForPlaylist(long userId, long trackId)
        {
            var playlist = context.Playlists.FirstOrDefault(p => p.PlaylistTracks.Any(t => t.Id == trackId));
            var count = playlist.PlaylistTracks.SelectMany(p => p.Votes).Count(v => v.User.Id == userId);
            return count;
        }

        public Vote ReadVoteOfUserFromPlaylistTrack(long userId, long trackId)
        {
            var track = context.PlaylistTracks.FirstOrDefault(pt => pt.Id == trackId);
            var vote = track?.Votes?.FirstOrDefault(v => v.User.Id == userId);
            return vote;
        }

        public Playlist UpdatePlaylist(Playlist playlist)
        {
            var originalPlaylist = context.Playlists.Find(playlist.Id);

            context.Entry(originalPlaylist).CurrentValues.SetValues(playlist);
            context.Entry(originalPlaylist).State = EntityState.Modified;
            context.SaveChanges();

            return originalPlaylist;
        }

        public PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack)
        {
            var originalTrack = context.PlaylistTracks.Find(playlistTrack.Id);
            if (originalTrack == null) return null;

            context.Entry(originalTrack).CurrentValues.SetValues(playlistTrack);
            context.Entry(originalTrack).State = EntityState.Modified;
            context.SaveChanges();

            return originalTrack;
        }

        public bool SetPlaylistTrackPlayedAtTimestamp(long playlistTrackId)
        {
            var track = context.Playlists.SelectMany(p => p.PlaylistTracks).FirstOrDefault(t => t.Id == playlistTrackId);
            if (track == null) return false;

            track.PlayedAt = DateTime.Now;

            return context.SaveChanges() > 0;
        }

        public Vote UpdateVote(Vote vote)
        {
            context.Votes.Attach(vote);
            context.Entry(vote).State = EntityState.Modified;
            context.SaveChanges();
            return vote;
        }
    }
}
