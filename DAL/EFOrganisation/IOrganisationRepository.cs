using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System.Collections.Generic;

namespace BB.DAL.EFOrganisation
{
    public interface IOrganisationRepository
    {
        //Organisation
        Organisation CreateOrganisation(Organisation organisation, User user);
        Organisation UpdateOrganisation(Organisation organisation);
        IEnumerable<Organisation> ReadOrganisations();
        Organisation ReadOrganisation(long organisationId);
        Organisation ReadOrganisation(string organisationName);
        IEnumerable<Organisation> ReadOrganisationsForUser(long userId);
        void DeleteOrganisation(long organisationId);
        Organisation ReadOrganisationForPlaylist(long playlistId);
        //DashboardBlock
        DashboardBlock CreateDashboardBlock(DashboardBlock dashboardBlock);
        DashboardBlock UpdateDashboardBlock(DashboardBlock block);
        IEnumerable<DashboardBlock> ReadDashboardBlocks(Organisation organisation);
        void DeleteDashboardBlock(long blockId);
        
    }
}
