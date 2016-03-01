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

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class PlaylistControllerTest
    {
        private PlaylistController controller;
        private IPlaylistManager playlistManager;

        [TestInitialize]
        public void TestInitialize()
        {
            controller = new PlaylistController(ContextEnum.BeatBuddyTest);
            playlistManager = new PlaylistManager(ContextEnum.BeatBuddyTest);
            DbInitializer.Initialize();
        }

        [TestMethod]
        public void TestSearchAndAddTrack()
        {
            var result = controller.SearchTrack("kshmr - bazaar");
            var tracks = result.Data as List<Track>;

            Assert.IsNotNull(tracks);
            Assert.IsTrue(tracks.Any(t => t.Title.ToString().ToLower().Contains("bazaar")));

            var addTrackResult = controller.AddTrack(1, tracks.First().TrackSource.TrackId) as HttpStatusCodeResult;

            Assert.IsNotNull(addTrackResult);
            Assert.AreEqual(200, addTrackResult.StatusCode);

            var playlistResult = controller.View(1) as ViewResult;

            Assert.IsNotNull(playlistResult);
            
            var playlist = playlistResult.Model as Playlist;

            Assert.IsNotNull(playlist);
            Assert.AreEqual(2, playlist.PlaylistTracks.Count);
        }

        [TestMethod]
        public void TestSearchAndAddDuplicate()
        {
            var addTrackResult = controller.AddTrack(1, "dQw4w9WgXcQ") as HttpStatusCodeResult;

            Assert.IsNotNull(addTrackResult);
            Assert.AreEqual(400, addTrackResult.StatusCode);
            Assert.AreEqual("You can not add a song that is already in the list", addTrackResult.StatusDescription);
        }

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
            var addTrackResult = controller.MoveTrackToHistory(1) as HttpStatusCodeResult;

            Assert.IsNotNull(addTrackResult);
            Assert.AreEqual(200, addTrackResult.StatusCode);

            var playlistResult = controller.View(1) as ViewResult;

            Assert.IsNotNull(playlistResult);

            var playlist = playlistResult.Model as Playlist;

            Assert.IsNotNull(playlist);
            Assert.AreEqual(1, playlist.PlaylistTracks.Count); // after moving the one and only track to the history, the tracks in the playlist view should be empty
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
            Playlist playlist = playlistManager.ReadPlaylist("Awesome party");
            Assert.AreEqual(playlist.Active, true);
            
        }
    }
}
