using System;
using System.Collections.Generic;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.BL.Domain;
using BB.DAL.EFPlaylist;
using System.Linq;

namespace BB.BL
{
    public class PlaylistManager : IPlaylistManager
    {
        private readonly IPlaylistRepository repo;

        public PlaylistManager(IPlaylistRepository playlistRepository)
        {
            this.repo = playlistRepository;
        }
        public Comment CreateComment(string text, User user)
        {
            var comment = new Comment
            {
                Text = text,
                User = user,
                TimeStamp = DateTime.Now
            };
            return repo.CreateComment(comment);
        }

        public Playlist CreatePlaylistForUser(string name, string description, string key, int maxVotesPerUser, bool active, string imageUrl, User createdBy)
        {
            var playlist = new Playlist
            {
                Name = name,
                Description = description,
                Key = key,
                MaximumVotesPerUser = maxVotesPerUser,
                Active = active,
                ImageUrl = imageUrl,
                CreatedById = createdBy.Id,
                ChatComments = new List<Comment>(),
                Comments = new List<Comment>(),
                PlaylistTracks = new List<PlaylistTrack>()
            };
            return repo.CreatePlaylist(playlist);
        }

        public Playlist CreatePlaylistForOrganisation(string name, string description, string key, int maxVotesPerUser, bool active, string imageUrl, User createdBy, Organisation organisation)
        {
            Playlist playlist = new Playlist()
            {
                Name = name,
                Description = description,
                Key = key,
                MaximumVotesPerUser = maxVotesPerUser,
                Active = active,
                ImageUrl = imageUrl,
                CreatedById = createdBy.Id,
                ChatComments = new List<Comment>(),
                Comments = new List<Comment>(),
                PlaylistTracks = new List<PlaylistTrack>()
            };
            return repo.CreatePlaylist(playlist, organisation);
        }


        public IEnumerable<Playlist> ReadPlaylistsForUser(long userId)
        {
            return repo.ReadPlaylistsForUser(userId);
        }

        public PlaylistTrack CreatePlaylistTrack(Track track)
        {
            PlaylistTrack playlistTrack = new PlaylistTrack()
            {
                Track = track,
                PlayedAt = null,
                Votes = new List<Vote>()
            };
            return repo.CreatePlaylistTrack(playlistTrack);
        }

        public TrackSource CreateTrackSource(SourceType sourceType, string url)
        {
            TrackSource trackSource = new TrackSource()
            {
                SourceType = sourceType,
                Url = url
            };
            return repo.CreateTrackSource(trackSource);
        }

        public bool CheckIfReachedMaximumVotes(long userId, long trackId) {
            return repo.ReadNumberOfVotesOfUserForPlaylist(userId, trackId) >= repo.ReadMaximumVotesPerUser(trackId);
        }

        public Vote CreateVote(int score, long userId, long trackId)
        {
            Vote vote;
            if (repo.ReadVoteOfUserFromPlaylistTrack(userId,trackId) != null) {
                var existingVote = repo.ReadVoteOfUserFromPlaylistTrack(userId, trackId);
                if (existingVote.Score == score)
                {
                    DeleteVote(existingVote);
                    return null;
                }
                else {
                    vote = existingVote;
                    vote.Score = score;
                    return repo.CreateVote(vote, userId, trackId);
                }
            }
            vote = new Vote()
            {
                Score = score
            };
            return repo.CreateVote(vote, userId, trackId);
        }

        public void DeleteVote(long voteId)
        {
            repo.DeleteVote(voteId);
        }

        public void DeleteVote(Vote vote)
        {
            repo.DeleteVote(vote);
        }

        public void DeleteComment(long commentId)
        {
            repo.DeleteComment(commentId);
        }

        public IEnumerable<Playlist> ReadPlaylists(long userId)
        {
            return repo.ReadPlaylists(userId);
        }

        public Playlist DeletePlaylist(long playlistId)
        {
            return repo.DeletePlaylist(playlistId);
        }

        public void DeletePlaylistTrack(long playlistTrackId)
        {
            repo.DeletePlaylistTrack(playlistTrackId);
        }

        public bool MarkTrackAsPlayed(long playlistTrackId)
        {
            return repo.SetPlaylistTrackPlayedAtTimestamp(playlistTrackId);
        }

        public Playlist UpdatePlaylist(Playlist playlist, string email)
        {
            return repo.UpdatePlaylist(playlist, email);
        }

        public Track AddTrackToPlaylist(long playlistId, Track track)
        {
            return repo.CreateTrack(playlistId, track);
        }

        public void DeleteTrack(long trackId)
        {
            repo.DeleteTrack(trackId);
        }

        public void DeleteTrackSource(long trackSourceId)
        {
            repo.DeleteTrackSource(trackSourceId);
        }

        public void DeleteVote(long playlistTrackId, long userId)
        {
            var playlist = repo.ReadPlaylistTrack(playlistTrackId);

            var vote = repo.ReadPlaylistTrack(playlistTrackId).Votes.First(v => v.User.Id == userId);
            repo.DeleteVote(vote.Id);
        }

        public IEnumerable<Comment> ReadChatComments(Playlist playlist)
        {
            return repo.ReadChatComments(playlist);
        }

        public IEnumerable<Comment> ReadComments(Playlist playlist)
        {
            return repo.ReadComments(playlist);
        }

        public Playlist ReadPlaylist(string name)
        {
            return repo.ReadPlaylist(name);
        }

        public Playlist ReadPlaylist(long playlistId)
        {
            return repo.ReadPlaylist(playlistId);
        }

        public IEnumerable<Playlist> ReadPlaylists()
        {
            return repo.ReadPlaylists();
        }

        public IEnumerable<Playlist> ReadPlaylists(Organisation organisation)
        {
            return repo.ReadPlaylists(organisation);
        }

        public bool CheckIfUserCreatedPlaylist(long playlistId, long userId)
        {
            Playlist playlist = ReadPlaylist(playlistId);
            if (playlist.CreatedById == userId) { return true; }
            return false;
        }

        public PlaylistTrack ReadPlaylistTrack(long playlistTrackId)
        {
            return repo.ReadPlaylistTrack(playlistTrackId);
        }

        public IEnumerable<PlaylistTrack> ReadPlaylistTracks(Playlist playlist)
        {
            return repo.ReadPlaylistTracks(playlist);
        }

        public Track ReadTrack(long trackId)
        {
            return repo.ReadTrack(trackId);
        }

        public IEnumerable<Track> ReadTracks()
        {
            return repo.ReadTracks();
        }

        public TrackSource ReadTrackSource(long trackSourceId)
        {
            return repo.ReadTrackSource(trackSourceId);
        }

        public IEnumerable<TrackSource> ReadTrackSources()
        {
            return repo.ReadTrackSources();
        }

        public Vote ReadVote(long voteId)
        {
            return repo.ReadVote(voteId);
        }

        public IEnumerable<Vote> ReadVotesForPlaylist(Playlist playlist)
        {
            return repo.ReadVotesForPlaylist(playlist);
        }

        public IEnumerable<Vote> ReadVotesForUser(User user) {
            return repo.ReadVotesUser(user);
        }

        public Comment UpdateComment(Comment comment)
        {
            return repo.UpdateComment(comment);
        }

        public Playlist UpdatePlaylist(Playlist playlist)
        {
            return repo.UpdatePlaylist(playlist);
        }

        public PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack)
        {
            return repo.UpdatePlayListTrack(playlistTrack);
        }

        public Track UpdateTrack(Track track)
        {
            return repo.UpdateTrack(track);
        }

        public TrackSource UpdateTracksource(TrackSource trackSource)
        {
            return repo.UpdateTracksource(trackSource);
        }

        public Vote UpdateVote(Vote vote)
        {
            return repo.UpdateVote(vote);
        }

    }
}
