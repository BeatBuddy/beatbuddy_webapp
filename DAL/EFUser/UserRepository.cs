using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Users;

namespace BB.DAL.EFUser
{
    public class UserRepository : IUserRepository
    {
        public User CreateUser(User user)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public User UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
