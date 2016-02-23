using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;

namespace BB.DAL.EFOrganisation
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private EFDbContext ctx;

        public OrganisationRepository(ContextEnum contextEnum)
        {
            ctx = new EFDbContext(contextEnum);
        }

        public DashboardBlock CreateDashboardBlock(DashboardBlock dashboardBlock)
        {
            throw new NotImplementedException();
        }

        public Organisation CreateOrganisation(Organisation organisation, User user)
        {
            organisation = ctx.Organisations.Add(organisation);
            ctx.SaveChanges();
            var user1 = ctx.User.Find(user.Id);
            var organisation1 = ctx.Organisations.Find(organisation.Id);
            UserRole userRole = new UserRole()
            {
                Organisation = organisation1,
                User = user1,
                Role = Role.Organiser
            };
            ctx.UserRole.Add(userRole);
            ctx.SaveChanges();
            return organisation;
        }

        public void DeleteDashboardBlock(long blockId)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrganisation(long organisationId)
        {
            throw new NotImplementedException();
        }

        public List<DashboardBlock> ReadDashboardBlocks(Organisation organisation)
        {
            throw new NotImplementedException();
        }

        public Organisation ReadOrganisation(string organisationName)
        {
            return ctx.Organisations.Single(o => o.Name.Equals(organisationName));
        }

        public Organisation ReadOrganisation(long organisationId)
        {
            return ctx.Organisations.Find(organisationId);
        }

        public IEnumerable<Organisation> ReadOrganisations()
        {
            return ctx.Organisations;
        }

        public DashboardBlock UpdateDashboardBlock(DashboardBlock block)
        {
            throw new NotImplementedException();
        }

        public Organisation UpdateOrganisation(Organisation organisation)
        {
            ctx.Entry(organisation).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
            return organisation;
        }
    }
}
