using System.Collections.ObjectModel;
using System.Data.Entity;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using System.Collections.Generic;

namespace BB.DAL
{
    public class EFDbTestInitializer : DropCreateDatabaseAlways<EFDbContext>
    {
        
        protected override void Seed(EFDbContext context)
        {
            var organisation = new Organisation()
            {
                Name = "Jonah's Songs",
                Playlists = new List<Playlist>()
            };
            context.Organisations.Add(organisation);
            var user = new User()
            {
                Email = "jonah@gmail.com",
                FirstName = "Jonah",
                LastName = "Jordan",
                Nickname = "Jonahtje123xoxo"
            };
            context.User.Add(user);

            context.SaveChanges();

            var user1 = context.User.Find(user.Id);
            var organisation1 = context.Organisations.Find(organisation.Id);
            var userRole = new UserRole()
            {
                Organisation = organisation1,
                User = user1,
                Role = Role.Organiser
            };
            context.UserRole.Add(userRole);

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
                        TrackId = "dQw4w9WgXcQ",
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
