using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain;
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

        public User ReadUser(string email)
        {
            throw new NotImplementedException();
        }

        public User ReadUser(long userId)
        {
            throw new NotImplementedException();
        }

        public User ReadUser(string lastname, string firstname)
        {
            throw new NotImplementedException();
        }

        public List<User> ReadUsers()
        {
            return ctx.User.ToList();
        }

        public User UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
