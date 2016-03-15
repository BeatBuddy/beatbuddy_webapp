using System;
using System.Collections.Generic;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.DAL.EFPlaylist;
using System.Linq;
using BB.DAL.EFUser;

namespace BB.BL
{
    public class PlaylistManager : IPlaylistManager
    {
        private readonly IPlaylistRepository repo;
        private readonly IUserRepository userRepo;

        public PlaylistManager(IPlaylistRepository playlistRepository, IUserRepository userRepo)
        {
            this.repo = playlistRepository;
            this.userRepo = userRepo;
        }

        public Comment CreateComment(long playlistId, string text, string userEmail)
        {
            var comment = new Comment
            {
                Text = text,
                User = userRepo.ReadUser(userEmail),
                TimeStamp = DateTime.Now
            };
            return repo.CreateComment(playlistId, comment);
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

        public Playlist CreatePlaylistForOrganisation(string name, string description, string key, int maxVotesPerUser, bool active, string imageUrl, User createdBy, long organisationId)
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
            return repo.CreatePlaylist(playlist, organisationId);
        }


        public IEnumerable<Playlist> ReadPlaylistsForUser(long userId)
        {
            return repo.ReadPlaylistsForUser(userId);
        }

        public bool CheckIfReachedMaximumVotes(long userId, long trackId) {
            return repo.ReadNumberOfVotesOfUserForPlaylist(userId, trackId) >= repo.ReadMaximumVotesPerUser(trackId);
        }

        public Track ReadTrack(long trackId)
        {
            return repo.ReadTrack(trackId);
        }

        public Vote CreateVote(int score, long userId, long trackId)
        {
            Vote vote;
            if (repo.ReadVoteOfUserFromPlaylistTrack(userId,trackId) != null) {
                var existingVote = repo.ReadVoteOfUserFromPlaylistTrack(userId, trackId);
                if (existingVote.Score == score)
                {
                    DeleteVote(existingVote);
                    existingVote.Score = 0;
                    return existingVote;
                }
                else {
                    vote = existingVote;
                    vote.Score = score;
                    return repo.UpdateVote(vote);
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

        public bool MarkTrackAsPlayed(long playlistTrackId, long playlistId)
        {
            return repo.MarkTrackAsPlayed(playlistTrackId, playlistId);
        }

        public Playlist UpdatePlaylist(Playlist playlist, string email)
        {
            return repo.UpdatePlaylist(playlist, email);
        }

        public Track AddTrackToPlaylist(long playlistId, Track track)
        {
            
            return repo.CreateTrack(playlistId, track);
        }

        public void DeleteVote(long playlistTrackId, long userId)
        {
            var playlist = repo.ReadPlaylistTrack(playlistTrackId);

            var vote = repo.ReadPlaylistTrack(playlistTrackId).Votes.First(v => v.User.Id == userId);
            repo.DeleteVote(vote.Id);
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

        public IEnumerable<Track> ReadTracks()
        {
            return repo.ReadTracks();
        }

        public IEnumerable<Vote> ReadVotesForUser(User user)
        {
            return repo.ReadVotesUser(user);
        }

        public Playlist UpdatePlaylist(Playlist playlist)
        {
            return repo.UpdatePlaylist(playlist);
        }

        public PlaylistTrack UpdatePlayListTrack(PlaylistTrack playlistTrack)
        {
            return repo.UpdatePlayListTrack(playlistTrack);
        }

        public Vote UpdateVote(Vote vote)
        {
            return repo.UpdateVote(vote);
        }

    }
}
