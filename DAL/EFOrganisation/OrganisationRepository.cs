using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;

namespace BB.DAL.EFOrganisation
{
    public class OrganisationRepository : IOrganisationRepository
    {
        public DashboardBlock CreateDashboardBlock(DashboardBlock dashboardBlock)
        {
            throw new NotImplementedException();
        }

        public Organisation CreateOrganisation(Organisation organisation)
        {
            throw new NotImplementedException();
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

        public Organisation ReadOrganisation(long organisationId)
        {
            throw new NotImplementedException();
        }

        public List<Organisation> ReadOrganisations()
        {
            throw new NotImplementedException();
        }

        public DashboardBlock UpdateDashboardBlock(DashboardBlock block)
        {
            throw new NotImplementedException();
        }

        public Organisation UpdateOrganisation(Organisation organisation)
        {
            throw new NotImplementedException();
        }
    }
}
