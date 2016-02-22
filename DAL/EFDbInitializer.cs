using System.Data.Entity;
using BB.BL.Domain.Playlists;

namespace BB.DAL
{
    internal class EFDbInitializer : DropCreateDatabaseIfModelChanges<EFDbContext>
    {
        protected override void Seed(EFDbContext context)
        {
            context.SaveChanges();
        }
    }
}
