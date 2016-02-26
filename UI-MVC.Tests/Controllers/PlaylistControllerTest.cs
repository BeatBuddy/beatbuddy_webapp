using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
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
            var result = controller.SearchTrack("Rick Astley - Never Gonna Give You Up");
            var tracks = result.Data as List<Track>;

            Assert.IsNotNull(tracks);
            Assert.IsTrue(tracks.Any(t => t.Title.ToString().ToLower().Contains("never gonna give you up")));

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
        public void ViewPlaylist()
        {
            var result = controller.View(1) as ViewResult;

            Assert.IsNotNull(result);

            var tracks = result.Model as Playlist;

            Assert.IsNotNull(tracks);
            Assert.IsTrue(tracks.PlaylistTracks.Count > 0);
        }

        [TestMethod]
        public void CreatePlaylist()
        {
            PlaylistViewModel playlistViewModel = new PlaylistViewModel()
            {
                MaximumVotesPerUser = 4,
                Name = "Awesome party",
                Organisation = "Jonah's Songs",
                PlaylistMaster = "jonah@gmail.com"
            };

            RedirectToRouteResult viewResult = (RedirectToRouteResult)controller.Create(playlistViewModel, null);
            Playlist playlist = playlistManager.ReadPlaylist("Awesome party");
            Assert.AreEqual(playlist.Active, true);
            
        }
    }
}
