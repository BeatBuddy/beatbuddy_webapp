using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.DAL;
using BB.DAL.EFPlaylist;
using BB.DAL.EFUser;
using BB.UI.Web.MVC.Controllers.Web_API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTested.WebApi;
using MyTested.WebApi.Builders.Contracts.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Description;
using System.Web.UI.WebControls.Expressions;
using BB.UI.Web.MVC.Models;
using MyTested.WebApi.Common.Extensions;
using System.Collections;

namespace BB.UI.Web.MVC.Tests.Controllers.WebApi
{
    [TestClass]
    public class PlaylistControllerTest
    {
        IPlaylistManager playlistManager;
        IUserManager userManager;
        ITrackProvider trackProvider;
        IAlbumArtProvider albumArtProvider;

        User user;
        Playlist playlist;
        
        private Track _addedMetallicaTrack;
        private Track _addedNickelbackTrack;
        

        IAndControllerBuilder<PlaylistController> playlistControllerWithAuthenticatedUser;


        [TestInitialize]
        public void Initialize() {
            playlistManager = new PlaylistManager(new PlaylistRepository(new EFDbContext(ContextEnum.BeatBuddyTest)), new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            user = userManager.CreateUser("testemail@gmail.com", "matthias", "test", "acidshards", "");
            playlist = playlistManager.CreatePlaylistForUser("testplaylist", "gekke playlist om te testen", "125", 5, true, "", user);

            trackProvider = new YouTubeTrackProvider();
            albumArtProvider = new BingAlbumArtProvider();

            playlistControllerWithAuthenticatedUser = MyWebApi.Controller<PlaylistController>()
               .WithResolvedDependencyFor<IPlaylistManager>(playlistManager)
               .WithResolvedDependencyFor<IUserManager>(userManager)
               .WithResolvedDependencyFor<ITrackProvider>(trackProvider)
               .WithResolvedDependencyFor<IAlbumArtProvider>(albumArtProvider)
               .WithAuthenticatedUser(
                u => u.WithIdentifier("NewId")
                      .WithUsername(user.Email)
                      .WithClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email))
                      .WithClaim(new System.Security.Claims.Claim("sub", user.Email))
               );
            Track metallicaTrack = new Track()
            {
                Artist = "Metallica",
                Title = "Master of Puppets (live)",
                CoverArtUrl = "",
                Duration = 800,
                TrackSource = new TrackSource
                {
                    SourceType = SourceType.YouTube,
                    Url = "https://www.youtube.com/watch?v=kV-2Q8QtCY4",
                    TrackId = "kV-2Q8QtCY4"
                }
            };
            Track nickelbackTrack = new Track()
            {
                Artist = "Nickelback",
                Title = "How You Remind Me",
                CoverArtUrl = "",
                Duration = 400,
                TrackSource = new TrackSource
                {
                    SourceType = SourceType.YouTube,
                    Url = "https://www.youtube.com/watch?v=1cQh1ccqu8M",
                    TrackId = "1cQh1ccqu8M"
                }
            };

            _addedMetallicaTrack = playlistManager.AddTrackToPlaylist(playlist.Id, metallicaTrack);
            _addedNickelbackTrack = playlistManager.AddTrackToPlaylist(playlist.Id, nickelbackTrack);
            
        }

        [TestMethod]
        public void GetPlaylistTest()
        {
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<IPlaylistManager>(playlistManager)
                .Calling(c => c.getPlaylist(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Playlist>()
                .Passing(
                    p => p.Id == playlist.Id
                         && p.Description == playlist.Description
                         && p.Active == playlist.Active
                         && p.ChatComments.Equals(playlist.ChatComments)
                         && p.Comments.Equals(playlist.Comments)
                         && p.ImageUrl == playlist.ImageUrl
                         && p.Key == playlist.Key
                         && p.MaximumVotesPerUser == playlist.MaximumVotesPerUser
                         && p.Name == playlist.Name
                         && p.PlaylistMasterId == playlist.PlaylistMasterId
                         && p.PlaylistTracks.Equals(playlist.PlaylistTracks)
                );
        }

        [TestMethod]
        public void GetWrongPlaylistTest()
        {
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<IPlaylistManager>(playlistManager)
                .Calling(c => c.getPlaylist(-1))
                .ShouldReturn()
                .NotFound();
        }

        [TestMethod]
        public void LookupPlaylistByKeyTest()
        {
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<IPlaylistManager>(playlistManager)
                .Calling(c => c.getPlaylistByKey(playlist.Key))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Playlist>()
                .Passing(
                    p => p.Id == playlist.Id
                    && p.Description == playlist.Description
                    && p.Active == playlist.Active
                    && p.ChatComments.Equals(playlist.ChatComments)
                    && p.Comments.Equals(playlist.Comments)
                    && p.ImageUrl == playlist.ImageUrl
                    && p.Key == playlist.Key
                    && p.MaximumVotesPerUser == playlist.MaximumVotesPerUser
                    && p.Name == playlist.Name
                    && p.PlaylistMasterId == playlist.PlaylistMasterId
                    && p.PlaylistTracks.Equals(playlist.PlaylistTracks)
                );
        }

        [TestMethod]
        public void LookupPlaylistByWrongKeyTest()
        {
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<IPlaylistManager>(playlistManager)
                .Calling(c => c.getPlaylistByKey("doesnotexist"))
                .ShouldReturn()
                .NotFound();
        }

        [TestMethod]
        public void GetLivePlaylistWithNoTracksPlayedOrVoted()
        {
            List<LivePlaylistTrackViewModel> livePlaylistTracks = new List<LivePlaylistTrackViewModel>();
            foreach (var playlistTrack in playlist.PlaylistTracks)
            {
                playlistTrack.Votes = new List<Vote>();
                livePlaylistTracks.Add(new LivePlaylistTrackViewModel()
                {
                    Id = playlistTrack.Id,
                    Track = playlistTrack.Track,
                    Score = 0,
                    PersonalScore = 0
                });
            }
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getLivePlaylist(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(new LivePlaylistViewModel()
            {
                Id = playlist.Id,
                PlaylistTracks = livePlaylistTracks,
                Active = playlist.Active,
                ChatComments = playlist.ChatComments,
                Comments = playlist.Comments,
                CreatedById = playlist.CreatedById,
                Description = playlist.Description,
                ImageUrl = playlist.ImageUrl,
                Key = playlist.Key,
                MaximumVotesPerUser = playlist.MaximumVotesPerUser,
                PlaylistMasterId = playlist.PlaylistMasterId,
                Name = playlist.Name
            });
        }

        [TestMethod]
        public void GetLivePlaylistWithOneTrackPlayed()
        {
            var playedTrack = playlist.PlaylistTracks.First();
            playlistManager.MarkTrackAsPlayed(playedTrack.Id, playlist.Id);
            var notPlayedTrack = playlist.PlaylistTracks.Last();
       
            List<LivePlaylistTrackViewModel> livePlaylistTracks = new List<LivePlaylistTrackViewModel>();
            livePlaylistTracks.Add(
                new LivePlaylistTrackViewModel()
                {
                Id = notPlayedTrack.Id,
                    Track = notPlayedTrack.Track,
                    Score = 0,
                    PersonalScore = 0
                });
            
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getLivePlaylist(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(new LivePlaylistViewModel()
                {
                    Id = playlist.Id,
                    PlaylistTracks = livePlaylistTracks,
                    Active = playlist.Active,
                    ChatComments = playlist.ChatComments,
                    Comments = playlist.Comments,
                    CreatedById = playlist.CreatedById,
                    Description = playlist.Description,
                    ImageUrl = playlist.ImageUrl,
                    Key = playlist.Key,
                    MaximumVotesPerUser = playlist.MaximumVotesPerUser,
                    PlaylistMasterId = playlist.PlaylistMasterId,
                    Name = playlist.Name
                });
        }

        [TestMethod]
        public void GetLivePlaylistWithAllTracksPlayed()
        {
            playlist.PlaylistTracks.ForEach(x => playlistManager.MarkTrackAsPlayed(x.Id,playlist.Id));
            List<LivePlaylistTrackViewModel> livePlaylistTracks = new List<LivePlaylistTrackViewModel>();

            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getLivePlaylist(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(new LivePlaylistViewModel()
                {
                    Id = playlist.Id,
                    PlaylistTracks = livePlaylistTracks,
                    Active = playlist.Active,
                    ChatComments = playlist.ChatComments,
                    Comments = playlist.Comments,
                    CreatedById = playlist.CreatedById,
                    Description = playlist.Description,
                    ImageUrl = playlist.ImageUrl,
                    Key = playlist.Key,
                    MaximumVotesPerUser = playlist.MaximumVotesPerUser,
                    PlaylistMasterId = playlist.PlaylistMasterId,
                    Name = playlist.Name
                });
        }

        [TestMethod]
        public void GetLivePlaylistWithNullPlaylist()
        {
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getLivePlaylist(-1))
                .ShouldReturn()
                .NotFound();
        }

        [TestMethod]
        public void GetLivePlaylistWithNoTracksPlayedButOneVoted()
        {
            var votedTrack = playlist.PlaylistTracks.First();
            var notVotedTrack = playlist.PlaylistTracks.Last();
            playlistManager.CreateVote(1, user.Id, votedTrack.Id);
            
            List<LivePlaylistTrackViewModel> livePlaylistTracks = new List<LivePlaylistTrackViewModel>();
            livePlaylistTracks.Add(new LivePlaylistTrackViewModel()
            {
                Id = votedTrack.Id,
                Track = votedTrack.Track,
                Score = 1,
                PersonalScore = 1
            });
            livePlaylistTracks.Add(new LivePlaylistTrackViewModel()
            {
                Id = notVotedTrack.Id,
                Track = notVotedTrack.Track,
                Score = 0,
                PersonalScore = 0
            });
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getLivePlaylist(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(new LivePlaylistViewModel()
                {
                    Id = playlist.Id,
                    PlaylistTracks = livePlaylistTracks,
                    Active = playlist.Active,
                    ChatComments = playlist.ChatComments,
                    Comments = playlist.Comments,
                    CreatedById = playlist.CreatedById,
                    Description = playlist.Description,
                    ImageUrl = playlist.ImageUrl,
                    Key = playlist.Key,
                    MaximumVotesPerUser = playlist.MaximumVotesPerUser,
                    PlaylistMasterId = playlist.PlaylistMasterId,
                    Name = playlist.Name
                })
                .Passing( lp => lp.PlaylistTracks.First().Id == votedTrack.Id);
        }

        [TestMethod]
        public void GetHistoryTestWithOneTrackInHistory()
        {
            var playedTrack = playlist.PlaylistTracks.First();
            playlistManager.MarkTrackAsPlayed(playedTrack.Id, playlist.Id);

            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getHistory(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IEnumerable<PlaylistTrack>>()
                .Passing(pt => pt.Count() == 1
                               && pt.First().Id == playedTrack.Id
                               && pt.First().PlayedAt == playedTrack.PlayedAt
                               && pt.First().Playlist.Equals(playedTrack.Playlist)
                               && pt.First().PlaylistId == playedTrack.PlaylistId
                               && pt.First().Track.Equals(playedTrack.Track)
                               && pt.First().Votes.Equals(playedTrack.Votes));
        }

        [TestMethod]
        public void GetHistoryTestWithNoTracksInHistory()
        {
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getHistory(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IEnumerable<PlaylistTrack>>()
                .Passing(pt => !pt.Any());
        }

        [TestMethod]
        public void GetHistoryTestWithTwoTracksInHistory()
        {
            playlist.PlaylistTracks.ForEach(x => playlistManager.MarkTrackAsPlayed(x.Id, playlist.Id));

            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getHistory(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IEnumerable<PlaylistTrack>>()
                .Passing(pt => pt.Count() == 2);
        }

        [TestMethod]
        public void CreatePlaylistForUserTest()
        {
            FormDataCollection formData = new FormDataCollection(new List<KeyValuePair<string,string>>()
            {
                new KeyValuePair<string, string>("name","testplaylist"),
                new KeyValuePair<string, string>("description","andom description"),
                new KeyValuePair<string, string>("key","012345678")
            } );
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.createPlaylist(formData))
                .ShouldReturn()
                .Ok();
        }

        [TestMethod]
        public void SearchTrackTest() {
            var query = "Metallica - Master Of Puppets";
            MyWebApi.Controller<PlaylistController>()
                .Calling(c => c.searchTrack(query))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<List<Track>>();
        }

        [TestMethod]
        public void SearchNullTrackTest() {
            string query = null;
            MyWebApi.Controller<PlaylistController>()
                .Calling(c => c.searchTrack(query))
                .ShouldReturn()
                .NotFound();
        }

        [TestMethod]
        public void GetNextTrackTest()
        {
            var track = playlist.PlaylistTracks.First().Track;
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getNextTrack(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Track>()
                .Passing(t => t.Id == track.Id
                              && t.Artist == track.Artist
                              && t.CoverArtUrl == track.CoverArtUrl
                              && t.Duration == track.Duration
                              && t.Title == track.Title
                              );
        }

        [TestMethod]
        public void GetNextTrackWithAllTrackPlayed()
        {
            playlist.PlaylistTracks.ForEach(x => playlistManager.MarkTrackAsPlayed(x.Id, playlist.Id));

            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getNextTrack(playlist.Id))
                .ShouldReturn()
                .NotFound();
        }

        [TestMethod]
        public void GetYoutubePlaybackTrack()
        {
            var track = playlist.PlaylistTracks.First().Track;
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.getYoutubePlaybackTrack(track.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Track>()
                .Passing(t => t.Id == track.Id
                              && t.Artist == track.Artist
                              && t.CoverArtUrl == track.CoverArtUrl
                              && t.Duration == track.Duration
                              && t.Title == track.Title
                              );
        }

        [TestMethod]
        public void AddTrackTest()
        {
            playlistControllerWithAuthenticatedUser
                 .Calling(c => c.AddTrack(playlist.Id, _addedMetallicaTrack.TrackSource.TrackId))
                 .ShouldReturn()
                 .Ok()
                 .WithResponseModelOfType<Track>();
        }

        [TestMethod]
        public void AddNotFoundTrackTest()
        {
            playlistControllerWithAuthenticatedUser
                .Calling(c => c.AddTrack(playlist.Id, "willnotfoundthistrackId"))
                .ShouldReturn()
                .NotFound();
        }

        [TestMethod]
        public void AddTrackWithNonExistingPlaylistTest()
        {
            playlistControllerWithAuthenticatedUser
                .Calling(c => c.AddTrack(-1, _addedMetallicaTrack.TrackSource.TrackId))
                .ShouldReturn()
                .NotFound();
        }

        [TestMethod]
        public void Get2RecommendationsTest()
        {
            playlistControllerWithAuthenticatedUser
                .Calling(p => p.GetRecommendations(1))
                .ShouldReturn()
                .Ok();
        }

        [TestMethod]
        public void UpvoteTest() {
           playlistControllerWithAuthenticatedUser
                .Calling(c => c.Upvote(playlist.Id, _addedMetallicaTrack.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Vote>()
                .Passing(p => p.Score == 1);
        }

        [TestMethod]
        public void UpvoteDoubleTest()
        {
            playlistControllerWithAuthenticatedUser
                .Calling(c => c.Upvote(playlist.Id, _addedMetallicaTrack.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Vote>()
                .Passing(p => p.Score == 1);

            playlistControllerWithAuthenticatedUser
                .Calling(c => c.Upvote(playlist.Id, _addedMetallicaTrack.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Vote>()
                .Passing(p => p.Score == 0);
        }

        [TestMethod]
        public void UpvoteThanDownvoteTest()
        {
            playlistControllerWithAuthenticatedUser
                .Calling(c => c.Upvote(playlist.Id, _addedMetallicaTrack.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Vote>()
                .Passing(p => p.Score == 1);

            playlistControllerWithAuthenticatedUser
                .Calling(c => c.Downvote(playlist.Id, _addedMetallicaTrack.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Vote>()
                .Passing(p => p.Score == -1);
        }

        [TestMethod]
        public void DownvoteTest()
        {
            playlistControllerWithAuthenticatedUser
                 .Calling(c => c.Downvote(playlist.Id, _addedMetallicaTrack.Id))
                 .ShouldReturn()
                 .Ok()
                 .WithResponseModelOfType<Vote>()
                 .Passing(p => p.Score == -1);
        }

        [TestMethod]
        public void DownvoteDoubleTest()
        {
            playlistControllerWithAuthenticatedUser
                 .Calling(c => c.Downvote(playlist.Id, _addedMetallicaTrack.Id))
                 .ShouldReturn()
                 .Ok()
                 .WithResponseModelOfType<Vote>()
                 .Passing(p => p.Score == -1);

            playlistControllerWithAuthenticatedUser
                 .Calling(c => c.Downvote(playlist.Id, _addedMetallicaTrack.Id))
                 .ShouldReturn()
                 .Ok()
                 .WithResponseModelOfType<Vote>()
                 .Passing(p => p.Score == 0);
        }

        [TestCleanup]
        public void Cleanup()
        {
            playlistManager.DeletePlaylist(playlist.Id);
            userManager.DeleteUser(user.Id);
        }
    }
}
