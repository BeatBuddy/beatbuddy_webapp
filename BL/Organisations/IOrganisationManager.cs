using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System.Collections.Generic;

namespace BB.BL
{
    public interface IOrganisationManager
    {
        //Organisation
        Organisation CreateOrganisation(string name, string bannerUrl, string imageUrl ,string colorScheme, User organisator);
        Organisation UpdateOrganisation(Organisation organisation);
        IEnumerable<Organisation> ReadOrganisations();
        IEnumerable<Organisation> ReadOrganisations(long userId);
        Organisation ReadOrganisation(long organisationId);
        Organisation ReadOrganisation(string organisationName);
        void DeleteOrganisation(long organisationId);

        //DashboardBlock
        DashboardBlock CreateDashboardBlock(string blockName, int sequence);
        DashboardBlock UpdateDashboardBlock(DashboardBlock block);
        IEnumerable<DashboardBlock> ReadDashboardBlocks(Organisation organisation);
        void DeleteDashboardBlock(long blockId);
    }
}
