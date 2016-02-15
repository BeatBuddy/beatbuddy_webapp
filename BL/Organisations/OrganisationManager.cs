using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System.Collections.ObjectModel;
using BB.BL.Domain.Playlists;
using BB.DAL;
using BB.DAL.EFOrganisation;

namespace BB.BL
{
    public class OrganisationManager : IOrganisationManager
    {
        private IOrganisationRepository repo;
        public OrganisationManager(EFDbContext efDbContext)
        {
            repo = new OrganisationRepository(efDbContext);
        }
        public OrganisationManager()
        {
            repo = new OrganisationRepository();
        }
        public DashboardBlock CreateDashboardBlock(string blockName, int sequence)
        {
            DashboardBlock block = new DashboardBlock()
            {
                BlockName = blockName,
                Sequence = sequence
            };
            return repo.CreateDashboardBlock(block);
        }

        public Organisation CreateOrganisation(string name, string bannerUrl, string colorScheme, User organisator)
        {
            Organisation organisation = new Organisation
            {
                Name = name,
                BannerUrl = bannerUrl,
                ColorScheme = colorScheme,
                DashboardBlocks = new Collection<DashboardBlock>(),
                Playlists = new Collection<Playlist>(),
                Users = new Dictionary<User, Role> {{organisator, Role.Organiser}}
            };
            return repo.CreateOrganisation(organisation);
        }

        public void DeleteDashboardBlock(long blockId)
        {
            repo.DeleteDashboardBlock(blockId);
        }

        public void DeleteOrganisation(long organisationId)
        {
            repo.DeleteOrganisation(organisationId);
        }

        public List<DashboardBlock> ReadDashboardBlocks(Organisation organisation)
        {
            return repo.ReadDashboardBlocks(organisation);
        }

        public Organisation ReadOrganisation(long organisationId)
        {
            return repo.ReadOrganisation(organisationId);
        }

        public List<Organisation> ReadOrganisations()
        {
            return repo.ReadOrganisations().ToList();
        }

        public DashboardBlock UpdateDashboardBlock(DashboardBlock block)
        {
            return repo.UpdateDashboardBlock(block);
        }

        public Organisation UpdateOrganisation(Organisation organisation)
        {
            return repo.UpdateOrganisation(organisation);
        }
    }
}
