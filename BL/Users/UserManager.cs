using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Users;
using System.Collections.ObjectModel;
using BB.DAL.EFUser;
using BB.DAL;
using BB.BL.Domain.Playlists;

namespace BB.BL
{
    public class UserManager : IUserManager
    {
        private IUserRepository repo;

        public UserManager(EFDbContext context)
        {
            repo = new UserRepository(context);
        }

        public UserManager()
        {
            repo = new UserRepository();
        }
        public User CreateUser(string email, string lastname, string firstname, string nickname, string imageUrl)
        {
            User user = new User()
            {
                Email = email,
                LastName = lastname,
                FirstName = firstname,
                Nickname = nickname,
                ImageUrl = imageUrl,
                Roles = new Dictionary<Domain.Organisations.Organisation, Role>()
            };
            return repo.CreateUser(user);
        }

        public void DeleteUser(long userId)
        {
            repo.DeleteUser(userId);
        }

        public User ReadUser(string email)
        {
            return repo.ReadUser(email);
        }

        public User ReadUser(long userId)
        {
            return repo.ReadUser(userId);
        }

        public User ReadUser(string lastname, string firstname)
        {
            return repo.ReadUser(lastname, firstname);
        }

        public List<User> ReadUsers()
        {
            return repo.ReadUsers();
        }

        public User UpdateUser(User user)
        {
            return repo.UpdateUser(user);
        }
    }
}
