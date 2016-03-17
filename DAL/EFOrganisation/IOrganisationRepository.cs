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
        Organisation ReadOrganisationWithPlaylistsAndTracks(long organisationId);
        Organisation ReadOrganisationWithPlaylistsAndVotes(long organisationId);
        IEnumerable<Organisation> ReadOrganisationsForUser(long userId);
        IEnumerable<Organisation> SearchOrganisations(string prefix);
        Organisation DeleteOrganisation(long organisationId);
        Organisation ReadOrganisationForPlaylist(long playlistId);

        //DashboardBlock

    }
}
