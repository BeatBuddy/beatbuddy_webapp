using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;

namespace BB.DAL.EFUser
{
    public class UserRepository : IUserRepository
    {
        private EFDbContext ctx;
        

        public UserRepository(ContextEnum contextEnum)
        {
            ctx = new EFDbContext(contextEnum);
        }

        public User CreateUser(User user)
        {
            user = ctx.User.Add(user);
            ctx.SaveChanges();
            return user;
        }

        public UserRole CreateUserRole(UserRole userRole)
        {
            userRole = ctx.UserRole.Attach(userRole);
            ctx.SaveChanges();
            return userRole;
        }

        public UserRole CreateUserRole(long userId, long organisationId, Role role)
        {
            var org = ctx.Organisations.Find(organisationId);
            var user = ctx.User.Find(userId);
            UserRole userRole = new UserRole()
            {
                Organisation = org,
                User = user,
                Role = role
            };
            userRole = ctx.UserRole.Add(userRole);
            ctx.SaveChanges();
            return userRole;
        }

        public void DeleteUser(long userId)
        {
            throw new NotImplementedException();
        }

        public List<UserRole> ReadOrganisationsForUser(User user)
        {
            return ctx.UserRole.Include("Organisation").Include("User").Where(o => o.User.Id == user.Id).ToList();
        }

        public User ReadOrganiserFromOrganisation(Organisation organisation)
        {
            UserRole userRole = ctx.UserRole.Include("Organisation").Include("User").Where(o => o.Organisation.Id == organisation.Id).Single(a => a.Role == Role.Organiser);
            return ctx.User.Single(o => o.Id == userRole.User.Id);
        }

        public User ReadUser(string email)
        {
            return ctx.User.FirstOrDefault(u => u.Email.Equals(email));
        }

        public User ReadUser(long userId)
        {
            return ctx.User.Find(userId);
        }

        public User ReadUser(string lastname, string firstname)
        {
            throw new NotImplementedException();
        }

        public List<UserRole> ReadUserRolesForOrganisation(Organisation organisation)
        {
            return ctx.UserRole.Include("Organisation").Include("User").Where(o => o.Organisation.Id == organisation.Id).ToList();
        }

        public List<User> ReadUsers()
        {
            return ctx.User.ToList();
        }

        public User UpdateUser(User user)
        {
            ctx.Entry(user).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
            return user;
        }
    }
}
