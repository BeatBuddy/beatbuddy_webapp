using System.Collections.Generic;
using System.Linq;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.DAL.EFOrganisation;
using BB.DAL.EFUser;

namespace BB.BL
{
    public class OrganisationManager : IOrganisationManager
    {
        private readonly IOrganisationRepository organisationsRepository;
        private readonly IUserRepository userRepository;

        public OrganisationManager(ContextEnum contextEnum)
        {
            organisationsRepository = new OrganisationRepository(contextEnum);
            userRepository = new UserRepository(contextEnum);
        }

        public OrganisationManager()
        {
            organisationsRepository = new OrganisationRepository(ContextEnum.BeatBuddy);
            userRepository = new UserRepository(ContextEnum.BeatBuddy);
        }
        
        public DashboardBlock CreateDashboardBlock(string blockName, int sequence)
        {
            DashboardBlock block = new DashboardBlock
            {
                BlockName = blockName,
                Sequence = sequence
            };
            return organisationsRepository.CreateDashboardBlock(block);
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
            return organisationsRepository.CreateOrganisation(organisation, organisator);
        }

        public void DeleteDashboardBlock(long blockId)
        {
            organisationsRepository.DeleteDashboardBlock(blockId);
        }

        public void DeleteOrganisation(long organisationId)
        {
            organisationsRepository.DeleteOrganisation(organisationId);
        }

        public IEnumerable<DashboardBlock> ReadDashboardBlocks(Organisation organisation)
        {
            return organisationsRepository.ReadDashboardBlocks(organisation);
        }

        public Organisation ReadOrganisation(string organisationName)
        {
            return organisationsRepository.ReadOrganisation(organisationName);
        }

        public Organisation ReadOrganisation(long organisationId)
        {
            return organisationsRepository.ReadOrganisation(organisationId);
        }

        public IEnumerable<Organisation> ReadOrganisations()
        {
            return organisationsRepository.ReadOrganisations();
        }

        public IEnumerable<Organisation> ReadOrganisations(long userId)
        {
            return userRepository.ReadOrganisationsForUser(userId).Select(r => r.Organisation);
        }

        public DashboardBlock UpdateDashboardBlock(DashboardBlock block)
        {
            return organisationsRepository.UpdateDashboardBlock(block);
        }

        public Organisation UpdateOrganisation(Organisation organisation)
        {
            return organisationsRepository.UpdateOrganisation(organisation);
        }
    }
}
