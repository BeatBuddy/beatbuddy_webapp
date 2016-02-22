using System.Collections.ObjectModel;
using System.Data.Entity;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;

namespace BB.DAL
{
    class EFDbTestInitializer : DropCreateDatabaseAlways<EFDbContext>
    {

        protected override void Seed(EFDbContext context)
        {
            var organisation = new Organisation()
            {
                Name = "Jonah's Songs"
            };
            context.Organisations.Add(organisation);

            var playlist = new Playlist
            {
                Active = true,
                Name = "Liquid drum & bass"
            };

            var playlistTrack = new PlaylistTrack
            {
                Track = new Track
                {
                    Artist = "Fox Stevenson",
                    CoverArtUrl = "https://i.ytimg.com/vi/BjPJTU1KDCQ/hqdefault.jpg",
                    Title = "Come back",
                    TrackSource = new TrackSource
                    {
                        SourceType = SourceType.YouTube,
                        TrackId = "BjPJTU1KDCQ",
                        Url = "https://www.youtube.com/watch?v=BjPJTU1KDCQ"
                    }
                }
            };
            playlist.PlaylistTracks = new Collection<PlaylistTrack> { playlistTrack };

            context.Playlists.Add(playlist);
            context.SaveChanges();
        }
        
    }
}
