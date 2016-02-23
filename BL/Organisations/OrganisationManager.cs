using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System.Collections.ObjectModel;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.DAL;
using BB.DAL.EFOrganisation;

namespace BB.BL
{
    public class OrganisationManager : IOrganisationManager
    {
        private IOrganisationRepository repo;
        public OrganisationManager(ContextEnum contextEnum)
        {
            repo = new OrganisationRepository(contextEnum);
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

        public Organisation CreateOrganisation(string name, string bannerUrl,string imageUrl ,string colorScheme, User organisator)
        {
            Organisation organisation = new Organisation
            {
                Name = name,
                BannerUrl = bannerUrl,
                ImageUrl = imageUrl,
                ColorScheme = colorScheme,
                DashboardBlocks = new List<DashboardBlock>(),
                Playlists = new List<Playlist>(),
            };
            return repo.CreateOrganisation(organisation, organisator);
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

        public Organisation ReadOrganisation(string organisationName)
        {
            return repo.ReadOrganisation(organisationName);
        }

        public Organisation ReadOrganisation(long organisationId)
        {
            return repo.ReadOrganisation(organisationId);
        }

        public List<Organisation> ReadOrganisations()
        {
            return repo.ReadOrganisations().ToList();
        }

        public List<Organisation> ReadOrganisations(User user)
        {
            return null;
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
