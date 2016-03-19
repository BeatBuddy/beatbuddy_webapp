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
        
        Track addedtrack;
        

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
            Track track = new Track()
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

            addedtrack = playlistManager.AddTrackToPlaylist(playlist.Id, track);
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
                    && p.ChatComments == playlist.ChatComments
                    && p.Comments == playlist.Comments
                    && p.ImageUrl == playlist.ImageUrl
                    && p.Key == playlist.Key
                    && p.MaximumVotesPerUser == playlist.MaximumVotesPerUser
                    && p.Name == playlist.Name
                    && p.PlaylistMasterId == playlist.PlaylistMasterId
                    && p.PlaylistTracks == playlist.PlaylistTracks
                );
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
                    && p.ChatComments == playlist.ChatComments
                    && p.Comments == playlist.Comments
                    && p.ImageUrl == playlist.ImageUrl
                    && p.Key == playlist.Key
                    && p.MaximumVotesPerUser == playlist.MaximumVotesPerUser
                    && p.Name == playlist.Name
                    && p.PlaylistMasterId == playlist.PlaylistMasterId
                    && p.PlaylistTracks == playlist.PlaylistTracks
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

        /*
        [TestMethod]
        public void getHistoryTest() {
            Track trackForHistory = playlistManager.AddTrackToPlaylist(playlist.Id, new Track {
                Artist = "Matthias Heylen",
                CoverArtUrl = "",
                Duration = 312,
                Title = "Toplied",
                TrackSource = new TrackSource {
                    SourceType = SourceType.YouTube,
                    TrackId = "oyEuk8j8imI",playlist.Id))
                .ShouldReturn()
                    Url = "https://www.youtube.com/watch?v=oyEuk8j8imI"
                }
            });
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<PlaylistManager>(playlistManager)
                .WithResolvedDependencyFor<UserManager>(userManager)
                .WithAuthenticatedUser(
                 u => u.WithIdentifier("NewId")
                       .WithUsername(user.Email)
                       .WithClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email))
                       .WithClaim(new System.Security.Claims.Claim("sub", user.Email))
                       .InRoles("Admin", "User")
                )
                .Calling(c => c.getNextTrack(
                .Ok()
                .WithResponseModelOfType<Track>();
        }*/



        /*
        [TestMethod]
        public void AssignPlaylistMasterTest() {
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<PlaylistManager>(playlistManager)
                .WithResolvedDependencyFor<UserManager>(userManager)
                .Calling(c => c.AssignPlaylistMaster(playlist.Id, user.Id)) 
        }
        */

        /*
        [TestMethod]
        public void NextTrackTest()
        {
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<PlaylistManager>(playlistManager)
                .Calling(c => c.getNextTrack(playlist.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Track>()
                .Passing(
                    p => p.Id == 1
                );
        }*/
        [TestMethod]
        public void UpvoteTest() {
           playlistControllerWithAuthenticatedUser
                .Calling(c => c.Upvote(playlist.Id, addedtrack.Id))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Vote>();
        }

        [TestMethod]
        public void DownvoteTest()
        {
            playlistControllerWithAuthenticatedUser
                 .Calling(c => c.Downvote(playlist.Id, addedtrack.Id))
                 .ShouldReturn()
                 .Ok()
                 .WithResponseModelOfType<Vote>();
        }

        [TestMethod]
        public void AddTrackTest()
        {
            playlistControllerWithAuthenticatedUser
                 .Calling(c => c.AddTrack(playlist.Id, addedtrack.TrackSource.TrackId))
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
                .Calling(c => c.AddTrack(-1, addedtrack.TrackSource.TrackId))
                .ShouldReturn()
                .NotFound();
        }

        [TestCleanup]
        public void Cleanup()
        {
            playlistManager.DeletePlaylist(playlist.Id);
            userManager.DeleteUser(user.Id);
            
        }
        
        
    }
}
