using System.Data.Entity;
using BB.BL.Domain.Playlists;
using BB.DAL.EFPlaylist;

namespace BB.DAL
{
    public class EFDbInitializer : DropCreateDatabaseIfModelChanges<EFDbContext>
    {
        protected override void Seed(EFDbContext context)
        {
            context.SaveChanges();
        }
    }
}
