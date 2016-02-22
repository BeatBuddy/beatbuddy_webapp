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
        Organisation CreateOrganisation(string name, string bannerUrl, string imageUrl ,string colorScheme, User organisator);
        Organisation UpdateOrganisation(Organisation organisation);
        List<Organisation> ReadOrganisations();
        List<Organisation> ReadOrganisations(User user);
        Organisation ReadOrganisation(long organisationId);
        Organisation ReadOrganisation(string organisationName);
        void DeleteOrganisation(long organisationId);

        //DashboardBlock
        DashboardBlock CreateDashboardBlock(string blockName, int sequence);
        DashboardBlock UpdateDashboardBlock(DashboardBlock block);
        List<DashboardBlock> ReadDashboardBlocks(Organisation organisation);
        void DeleteDashboardBlock(long blockId);
    }
}
