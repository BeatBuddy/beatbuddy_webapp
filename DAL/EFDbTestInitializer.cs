using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;

namespace BB.DAL
{
    class EFDbTestInitializer : DropCreateDatabaseAlways<EFDbContext>
    {
        protected override void Seed(EFDbContext context)
        {
            Organisation organisation = new Organisation()
            {
                Name = "Jonah's Songs"
            };
            context.Organisations.Add(organisation);
            context.SaveChanges();
        }
    }
}
