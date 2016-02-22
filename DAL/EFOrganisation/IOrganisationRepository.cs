using BB.BL.Domain.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.DAL.EFOrganisation
{
    public interface IOrganisationRepository
    {
        //Organisation
        Organisation CreateOrganisation(Organisation organisation);
        Organisation UpdateOrganisation(Organisation organisation);
        IEnumerable<Organisation> ReadOrganisations();
        Organisation ReadOrganisation(long organisationId);
        Organisation ReadOrganisation(string organisationName);
        void DeleteOrganisation(long organisationId);

        //DashboardBlock
        DashboardBlock CreateDashboardBlock(DashboardBlock dashboardBlock);
        DashboardBlock UpdateDashboardBlock(DashboardBlock block);
        List<DashboardBlock> ReadDashboardBlocks(Organisation organisation);
        void DeleteDashboardBlock(long blockId);
    }
}
