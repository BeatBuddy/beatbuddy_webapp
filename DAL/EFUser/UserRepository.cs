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

        public void DeleteUser(long userId)
        {
            throw new NotImplementedException();
        }

        public List<UserRole> ReadOrganisationsForUser(User user)
        {
            return ctx.UserRole.Where(o => o.User.Id == user.Id).ToList();
        }

        public User ReadUser(string email)
        {
            return ctx.User.FirstOrDefault(u => u.Email.Equals(email));
        }

        public User ReadUser(long userId)
        {
            throw new NotImplementedException();
        }

        public User ReadUser(string lastname, string firstname)
        {
            throw new NotImplementedException();
        }

        public List<UserRole> ReadUserRolesForOrganisation(Organisation organisation)
        {
            return ctx.UserRole.Where(o => o.Organisation == organisation).ToList();
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
