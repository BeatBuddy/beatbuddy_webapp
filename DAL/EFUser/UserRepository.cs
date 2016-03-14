using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;

namespace BB.DAL.EFUser
{
    public class UserRepository : IUserRepository
    {
        private readonly EFDbContext context;
        
        public UserRepository(EFDbContext context)
        {
            this.context = context;
        }

        public User CreateUser(User user)
        {
            user = context.User.Add(user);
            context.SaveChanges();
            return user;
        }

        public UserRole CreateUserRole(UserRole userRole)
        {
            userRole = context.UserRole.Add(userRole);
            context.SaveChanges();
            return userRole;
        }

        public UserRole CreateUserRole(long userId, long organisationId, Role role)
        {
            var org = context.Organisations.Find(organisationId);
            var user = context.User.Find(userId);
            UserRole userRole = new UserRole()
            {
                Organisation = org,
                User = user,
                Role = role
            };
            userRole = context.UserRole.Add(userRole);
            context.SaveChanges();
            return userRole;
        }

        public void DeleteUser(User user)
        {
            context.User.Remove(user);
            context.SaveChanges();
        }

        public void DeleteUserRole(UserRole userRole)
        {
            context.UserRole.Remove(userRole);
            context.SaveChanges();
        }

        public IEnumerable<User> ReadCoOrganiserFromOrganisation(Organisation organisation)
        {
            IEnumerable<UserRole> userRoles = context.UserRole.Include("Organisation").Include("User").Where(o => o.Organisation.Id == organisation.Id).Where(a => a.Role == Role.Co_Organiser);

            List<User> users = new List<User>();
            foreach(UserRole userRole in userRoles)
            {
                users.Add(userRole.User);
            }
            return users.AsEnumerable();
        }

        public IEnumerable<UserRole> ReadOrganisationsForUser(long userId)
        {
            return context.UserRole.Include("Organisation").Include("User").Where(o => o.User.Id == userId);
        }

        public User ReadOrganiserFromOrganisation(Organisation organisation)
        {
            UserRole userRole = context.UserRole
                .Include("Organisation")
                .Include("User")
                .Where(o => o.Organisation.Id == organisation.Id).First();
            
            return context.User.SingleOrDefault(o => o.Id == userRole.User.Id);
        }

        public User ReadUser(string email)
        {
            return context.User.FirstOrDefault(u => u.Email.Equals(email));
        }

        public User ReadUser(long userId)
        {
            return context.User.FirstOrDefault(u => u.Id == userId);
        }

        public User ReadUser(string lastname, string firstname)
        {
            throw new NotImplementedException();
        }

        public UserRole ReadUserRoleForUserAndOrganisation(long userId, long organisationId)
        {
            IEnumerable<UserRole> userRoles = context.UserRole.Include("Organisation").Include("User").Where(u => u.User.Id == userId);
            if(userRoles != null)
                return userRoles.SingleOrDefault(o => o.Organisation.Id == organisationId);

            return null;
            
        }

        public IEnumerable<UserRole> ReadUserRolesForOrganisation(Organisation organisation)
        {
            return context.UserRole.Where(o => o.Organisation == organisation);
        }

        public IEnumerable<User> ReadUsers()
        {
            return context.User;
        }

        public User UpdateUser(User user)
        {
            context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return user;
        }
    }
}
