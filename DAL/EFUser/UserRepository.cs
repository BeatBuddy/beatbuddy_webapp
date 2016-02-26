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

        public void DeleteUser(long userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserRole> ReadOrganisationsForUser(long userId)
        {
            return context.UserRole.Where(o => o.User.Id == userId);
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
