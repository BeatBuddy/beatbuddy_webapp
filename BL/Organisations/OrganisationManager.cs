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
        public DashboardBlock CreateDashboardBlock(string blockName, int sequence)
        {
            DashboardBlock block = new DashboardBlock()
            {
                BlockName = blockName,
                Sequence = sequence
            };
            return repo.CreateDashboardBlock(block);
        }

        public Organisation CreateOrganisation(string name, string bannerUrl, string colorScheme, string key, User organisator)
        {
            Organisation organisation = new Organisation()
            {
                Name = name,
                BannerUrl = bannerUrl,
                ColorScheme = colorScheme,
                Key = key
            };
            organisation.DashboardBlocks = new Collection<DashboardBlock>();
            organisation.Playlists = new Collection<Playlist>();
            organisation.Users = new Dictionary<User, Role>();
            organisation.Users.Add(organisator, Role.Organiser);
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
            return repo.ReadOrganisations();
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
