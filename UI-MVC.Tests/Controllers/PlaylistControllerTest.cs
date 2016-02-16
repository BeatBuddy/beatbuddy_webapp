using System.Collections.Generic;
using System.Linq;
using BB.BL.Domain.Playlists;
using BB.UI.Web.MVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class PlaylistControllerTest
    {
        [TestMethod]
        public void SearchTrack()
        {
            PlaylistController controller = new PlaylistController();
            var result = controller.SearchTrack("Rick Astley - Never Gonna Give You Up");

            var tracks = result.Data as List<Track>;
            Assert.IsNotNull(tracks);
            Assert.IsTrue(tracks.Any(t => t.Title.ToString().ToLower().Contains("never gonna give you up")));
        }
    }
}
