using BB.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL
{
    public interface IUserManager
    {
        //User
        User CreateUser(string email, string lastname, string firstname, string nickname, string imageUrl);
        User UpdateUser(User user);
        User ReadUser(long userId);
        User ReadUser(string email);
        User ReadUser(string lastname, string firstname);
        List<User> ReadUsers();
        void DeleteUser(long userId);
    }
}
