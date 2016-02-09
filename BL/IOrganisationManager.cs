using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL
{
    public interface IOrganisationManager
    {
        //Organisation
        Organisation CreateOrganisation(string name, string bannerUrl, string colorScheme, string key, User organisator);
        Organisation UpdateOrganisation(Organisation organisation);
        List<Organisation> ReadOrganisations();
        Organisation ReadOrganisation(long organisationId);
        void DeleteOrganisation(long organisationId);

        //DashboardBlock
        DashboardBlock CreateDashboardBlock(string blockName, int sequence);
        DashboardBlock UpdateDashboardBlock(DashboardBlock block);
        List<DashboardBlock> ReadDashboardBlocks(Organisation organisation);
        void DeleteDashboardBlock(long blockId);
    }
}
