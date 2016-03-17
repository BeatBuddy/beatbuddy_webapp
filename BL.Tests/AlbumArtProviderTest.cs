using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.BL.Tests
{
    [TestClass]
    public class AlbumArtProviderTest
    {
        [TestMethod]
        public void TestAlbumSearch()
        {
            IAlbumArtProvider albumArtProvider = new BingAlbumArtProvider();
            string albumCoverUrl = albumArtProvider.Find("Mura Masa - Someday Somewhere");
            Assert.IsNotNull(albumCoverUrl);
            Assert.IsTrue(albumCoverUrl.StartsWith("http"));

            string albumCoverUrl2 = albumArtProvider.Find("Netsky - 2");
            Assert.IsNotNull(albumCoverUrl2);
            Assert.IsTrue(albumCoverUrl2.StartsWith("http"));
            Assert.AreNotEqual(albumCoverUrl, albumCoverUrl2);
        }

        [TestMethod]
        public void TestAlbumSearchWithGibberishSoHopefullyNoResult()
        {
            IAlbumArtProvider albumArtProvider = new BingAlbumArtProvider();
            string albumCoverUrl = albumArtProvider.Find("klsdjflkdsjflksdjflksdjflkjslkdjflksdjflksdjfkljsdlfkjdslkfjdsklfjsdlkfjklsdjflkdsjflksdjflksdjflksdjlkfjfsd");
            Assert.IsNull(albumCoverUrl);
        }
    }
}
