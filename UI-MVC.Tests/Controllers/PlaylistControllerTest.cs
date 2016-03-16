using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.UI.Web.MVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BB.UI.Web.MVC.Tests.Helpers;
using BB.UI.Web.MVC.Models;
using BB.BL;
using BB.BL.Domain.Users;
using BB.DAL;
using BB.DAL.EFOrganisation;
using BB.DAL.EFPlaylist;
using BB.DAL.EFUser;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class PlaylistControllerTest
    {
        private PlaylistController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            controller = new PlaylistController(DbInitializer.CreatePlaylistManager(),new YouTubeTrackProvider(), DbInitializer.CreateUserManager(), DbInitializer.CreateOrganisationManager(), new BingAlbumArtProvider());
            
            DbInitializer.Initialize();
        }

        [TestMethod]
        public void TestSearchAndAddTrack()
        {
            UserManager userManager = (UserManager) DbInitializer.CreateUserManager();
            PlaylistManager playlistManager = (PlaylistManager) DbInitializer.CreatePlaylistManager();
            var user = userManager.ReadUser("jonah@gmail.com");

            var playlistje = playlistManager.CreatePlaylistForUser("testPlaylist", "teste", "tesje", 2, true, null, user);
            var result = controller.SearchTrack("kshmr - bazaar");
            var tracks = result.Data as List<Track>;

            Assert.IsNotNull(tracks);
            Assert.IsTrue(tracks.Any(t => t.Title.ToString().ToLower().Contains("bazaar")));

            var addTrackResult = controller.AddTrack(playlistje.Id, tracks.First().TrackSource.TrackId) as HttpStatusCodeResult;

            Assert.IsNotNull(addTrackResult);
            Assert.AreEqual(200, addTrackResult.StatusCode);

            var playlistResult = controller.View(playlistje.Id) as ViewResult;

            Assert.IsNotNull(playlistResult);

            playlistje = playlistResult.Model as Playlist;

            Assert.IsNotNull(playlistje);
            Assert.AreEqual(1, playlistje.PlaylistTracks.Count);
        }

        /*
        [TestMethod]
        public void TestSearchAndAddDuplicate()
        {
            var addTrackResult = controller.AddTrack(1, "dQw4w9WgXcQ") as HttpStatusCodeResult;

            Assert.IsNotNull(addTrackResult);
            Assert.AreEqual(400, addTrackResult.StatusCode);
            Assert.AreEqual("You can not add a song that is already in the list", addTrackResult.StatusDescription);
        }
        */

        [TestMethod]
        public void ViewPlaylist()
        {
            var result = controller.View(1) as ViewResult;

            Assert.IsNotNull(result);

            var tracks = result.Model as Playlist;

            Assert.IsNotNull(tracks);
            Assert.IsTrue(tracks.PlaylistTracks.Count > 0);
        }

        [TestMethod]
        public void TestMoveTrackToHistory()
        {
            var playlistResult = controller.View(1) as ViewResult;
            var playlist = playlistResult?.Model as Playlist;
            var playlistTracks = playlist?.PlaylistTracks;
            var aantalTracks = playlistTracks?.Count ?? 0; // get the amount of tracks in the playlist
            
            var moveTrackResult = controller.MoveTrackToHistory(1) as HttpStatusCodeResult;
            Assert.IsNotNull(moveTrackResult);
            Assert.AreEqual(200, moveTrackResult.StatusCode);

            playlistResult = controller.View(1) as ViewResult;
            Assert.IsNotNull(playlistResult);

            playlist = playlistResult.Model as Playlist;
            Assert.IsNotNull(playlist);
            Assert.AreEqual(aantalTracks - 1, playlist.PlaylistTracks.Count); // after moving the track to the history, make sure it is one less track compared to in the beginning
        }



        [TestMethod]
        public void CreatePlaylist()
        {
            PlaylistViewModel playlistViewModel = new PlaylistViewModel()
            {
                MaximumVotesPerUser = 4,
                Name = "Awesome party"
            };

            RedirectToRouteResult viewResult = (RedirectToRouteResult)controller.Create(playlistViewModel, null);
            Playlist playlist = DbInitializer.CreatePlaylistManager().ReadPlaylist("Awesome party");
            Assert.AreEqual(playlist.Active, true);

        }
    }
}
