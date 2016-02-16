using System.Collections.Generic;
using System.Linq;
using BB.BL.Domain.Playlists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.BL.Tests
{
    [TestClass]
    class YouTubeTrackProviderTest
    {
        [TestMethod]
        public void testYoutubeSearch()
        {
            ITrackProvider youtubeProvider = new YouTubeTrackProvider();
            List<Track> tracks = youtubeProvider.Search("Rick Astley - Never Gonna Give You Up");

            var firstResult = tracks.FirstOrDefault(t => t.Title.ToLower().Contains("never gonna give you up"));
            Assert.IsNotNull(firstResult);
            Assert.AreEqual(firstResult.Artist.ToLower(), "rick astley");
            Assert.AreEqual(firstResult.TrackSource.SourceType, SourceType.YouTube);
            Assert.IsNotNull(firstResult.TrackSource.Url);
            Assert.IsNotNull(firstResult.CoverArtUrl);
        }

    }
}
