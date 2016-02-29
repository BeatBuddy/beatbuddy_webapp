using System;
using System.Collections.Generic;
using System.Linq;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;

namespace BB.DAL.EFUser
{
    public class UserRepository : IUserRepository
    {
        private readonly EFDbContext context;
        
        public UserRepository(ContextEnum contextEnum)
        {
            context = new EFDbContext(contextEnum);
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

        public void DeleteUser(long userId)
        {
            throw new NotImplementedException();
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
            UserRole userRole = context.UserRole.Include("Organisation").Include("User").Where(o => o.Organisation.Id == organisation.Id).Single(a => a.Role == Role.Organiser);
            return context.User.Single(o => o.Id == userRole.User.Id);
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
