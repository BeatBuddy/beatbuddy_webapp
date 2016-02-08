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
        List<Organisation> ReadOrganisations();
        Organisation ReadOrganisation(long organisationId);
        Organisation CreateOrganisation(string name, string bannerUrl, string colorScheme, string key, User organisator);
        void DeleteOrganisation(long organisationId);
        Organisation UpdateOrganisation(Organisation organisation);

        DashboardBlock CreateDashboardBlock(string blockName, int sequence);
        DashboardBlock UpdateDashboardBlock(DashboardBlock block);
        void DeleteDashboardBlock(long blockId);
    }
}
