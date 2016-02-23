using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;


namespace BB.DAL
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(ContextEnum test) : base(test.ToString())
        {
            if (test.Equals(ContextEnum.BeatBuddyTest))
            {
                Database.SetInitializer<EFDbContext>(new EFDbTestInitializer());
            }
            else
            {
                Database.SetInitializer<EFDbContext>(new EFDbInitializer());
            }
        }

        public DbSet<DashboardBlock> DashboardBlocks { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        //public DbSet<SourceType> SourceTypes { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackSource> TrackSources { get; set; }
        public DbSet<Vote> Votes { get; set; }
        //public DbSet<Role> roles { get; set; }
        public virtual DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organisation>().Property(p => p.Name).IsRequired();
                
        }
    }
}
