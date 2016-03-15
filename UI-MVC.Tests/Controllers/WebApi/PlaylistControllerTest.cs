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

namespace BB.UI.Web.MVC.Tests.Controllers.WebApi
{
    [TestClass]
    public class PlaylistControllerTest
    {
        PlaylistManager playlistManager;
        UserManager userManager;
        User user;
        Playlist playlist;

        [TestInitialize]
        public void Initialize() {
            playlistManager = new PlaylistManager(new PlaylistRepository(new EFDbContext(ContextEnum.BeatBuddyTest)), new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            user = userManager.CreateUser("testemail@gmail.com", "matthias", "test", "acidshards", "");
            playlist = playlistManager.CreatePlaylistForUser("testplaylist", "gekke playlist om te testen", "125", 5, true, "", user);

            Track track = new Track()
            {
                Artist = "Metallica",
                Title = "Master of Puppets (live)",
                CoverArtUrl = "",
                Duration = 800,
                TrackSource = new TrackSource
                {
                    SourceType = SourceType.YouTube,
                    Url = "https://www.youtube.com/watch?v=kV-2Q8QtCY4"
                }
            };
            Track addedtrack = playlistManager.AddTrackToPlaylist(playlist.Id, track);
        }

        [TestMethod]
        public void GetPlaylistTest()
        {
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<PlaylistManager>(playlistManager)
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
        public void GetWrongPlaylistTest()
        {
            MyWebApi.Controller<PlaylistController>()
                .WithResolvedDependencyFor<PlaylistManager>(playlistManager)
                .Calling(c => c.getPlaylist(playlist.Id-1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<Playlist>()
                .Passing(
                    p => p.Id != playlist.Id
                );
        }

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

        [TestCleanup]
        public void Cleanup() {
            userManager.DeleteUser(user.Id);
        }
        
        
    }
}
