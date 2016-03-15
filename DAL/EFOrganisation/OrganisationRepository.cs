using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;

namespace BB.DAL.EFOrganisation
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly EFDbContext context;

        public OrganisationRepository(EFDbContext context)
        {
            this.context = context;
        }

        public Organisation ReadOrganisationForPlaylist(long playlistId)
        {
            return context.Organisations.SingleOrDefault(o => o.Playlists.FirstOrDefault(p => p.Id == playlistId).Id == playlistId);
        }

        public Organisation CreateOrganisation(Organisation organisation, User user)
        {
            organisation = context.Organisations.Add(organisation);
            context.SaveChanges();
            var user1 = context.User.Find(user.Id);
            var organisation1 = context.Organisations.Find(organisation.Id);
            UserRole userRole = new UserRole
            {
                Organisation = organisation1,
                User = user1,
                Role = Role.Organiser
            };
            context.UserRole.Add(userRole);
            context.SaveChanges();
            return organisation;
        }

        public IEnumerable<Organisation> ReadOrganisationsForUser(long userId)
        {
            var userRoles = context.UserRole.Include("Organisation").Include("User").Where(ur => ur.User.Id == userId);
            return userRoles.Count() > 0 ? userRoles.Select(userRole => userRole.Organisation).ToList() : new List<Organisation>();
        }

        public Organisation DeleteOrganisation(long organisationId)
        {
           
            var org = ReadOrganisation(organisationId);
            var userRoles = context.UserRole.ToList().FindAll(p => p.Organisation == org);
            context.UserRole.RemoveRange(userRoles);
            org = context.Organisations.Remove(org);
            context.SaveChanges();
            return org;
        }

        public Organisation ReadOrganisation(string organisationName)
        {
            return context.Organisations.FirstOrDefault(o => o.Name.Equals(organisationName));
        }

        public Organisation ReadOrganisation(long organisationId)
        {
            return context.Organisations
                .Include(o => o.Playlists)
                .FirstOrDefault(o => o.Id == organisationId);
        }

        public IEnumerable<Organisation> ReadOrganisations()
        {
            return context.Organisations;
        }

        public Organisation UpdateOrganisation(Organisation organisation)
        {
            context.Entry(organisation).State = EntityState.Modified;
            context.SaveChanges();
            return organisation;
        }

        public IEnumerable<Organisation> SearchOrganisations(string prefix)
        {
            return context.Organisations.Where(p => p.Name.Contains(prefix));
        }
    }
}
