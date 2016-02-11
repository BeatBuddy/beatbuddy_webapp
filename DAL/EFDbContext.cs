using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;


namespace BB.DAL
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(): base("BeatBuddy")
        {
            Database.SetInitializer<EFDbContext>(new EFDbInitializer());
        }
        public DbSet<DashboardBlock> DashboardBlocks { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        //public DbSet<SourceType> SourceTypes { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackSource> TrackSources { get; set; }
        public DbSet<Vote> Votes { get; set; }
        //public DbSet<Role> roles { get; set; }
        public DbSet<User> User { get; set; }
    }
}
