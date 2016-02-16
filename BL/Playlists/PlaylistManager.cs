using System;
using System.Collections.Generic;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using System.Collections.ObjectModel;
using BB.BL.Domain;
using BB.DAL.EFPlaylist;

namespace BB.BL
{
    public class PlaylistManager : IPlaylistManager
    {
        private readonly IPlaylistRepository repo;
        public PlaylistManager()
        {
            repo = new PlaylistRepository(contextEnum);
        }
        public Comment CreateComment(string text, User user)
        {
            Comment comment = new Comment()
            {
                Text = text,
                User = user,
                TimeStamp = DateTime.Now
            };
            return repo.CreateComment(comment);
        }

        public Playlist CreatePlaylistForUser(string name, int maxVotesPerUser, bool active, string imageUrl, User playlistMaster, User createdBy)
        {
            Playlist playlist = new Playlist()
            {
                Name = name,
                MaximumVotesPerUser = maxVotesPerUser,
                Active = active,
                ImageUrl = imageUrl,
                PlaylistMasterId = playlistMaster.Id,
                CreatedById = createdBy.Id,
                ChatComments = new Collection<Comment>(),
                Comments = new Collection<Comment>(),
                PlaylistTracks = new Collection<PlaylistTrack>()
            };
            return repo.CreatePlaylist(playlist);
        }

        public PlaylistTrack CreatePlaylistTrack(Track track)
        {
            PlaylistTrack playlistTrack = new PlaylistTrack()
            {
                Track = track,
                AlreadyPlayed = false,
                Votes = new Collection<Vote>()
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

        public Vote CreateVote(int score, User user)
        {
            Vote vote = new Vote()
            {
                Score = score,
                User = user
            };
            return repo.CreateVote(vote);
        }

        public void DeleteComment(long commentId)
        {
            repo.DeleteComment(commentId);
        }

        public void DeletePlaylist(long playlistId)
        {
            repo.DeletePlaylist(playlistId);
        }

        public void DeletePlaylistTrack(long playlistTrackId)
        {
            repo.DeletePlaylistTrack(playlistTrackId);
        }

        public Track AddTrackToPlaylist(long playlistId, string artist, string title, TrackSource trackSource, string coverArtUrl)
        {
            var track = new Track()
            {
                Artist = artist,
                Title = title,
                TrackSource = trackSource,
                CoverArtUrl = coverArtUrl
            };
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

        public void DeleteVote(long voteId)
        {
            repo.DeleteVote(voteId);
        }

        public List<Comment> ReadChatComments(Playlist playlist)
        {
            return repo.ReadChatComments(playlist);
        }

        public List<Comment> ReadComments(Playlist playlist)
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

        public List<Playlist> ReadPlaylists()
        {
            return repo.ReadPlaylists();
        }

        public List<Playlist> ReadPlaylists(Organisation organisation)
        {
            return repo.ReadPlaylists(organisation);
        }

        public PlaylistTrack ReadPlaylistTrack(long playlistTrackId)
        {
            return repo.ReadPlaylistTrack(playlistTrackId);
        }

        public List<PlaylistTrack> ReadPlaylistTracks(Playlist playlist)
        {
            return repo.ReadPlaylistTracks(playlist);
        }

        public Track ReadTrack(long trackId)
        {
            return repo.ReadTrack(trackId);
        }

        public List<Track> ReadTracks()
        {
            return repo.ReadTracks();
        }

        public TrackSource ReadTrackSource(long trackSourceId)
        {
            return repo.ReadTrackSource(trackSourceId);
        }

        public List<TrackSource> ReadTrackSources()
        {
            return repo.ReadTrackSources();
        }

        public Vote ReadVote(long voteId)
        {
            return repo.ReadVote(voteId);
        }

        public List<Vote> ReadVotesForPlaylist(Playlist playlist)
        {
            return repo.ReadVotesForPlaylist(playlist);
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
