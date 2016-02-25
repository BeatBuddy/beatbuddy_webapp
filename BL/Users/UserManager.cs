using System.Collections.Generic;
using BB.BL.Domain.Users;
using BB.BL.Domain;
using BB.DAL.EFUser;
using BB.BL.Domain.Organisations;

namespace BB.BL
{
    public class UserManager : IUserManager
    {
        private IUserRepository repo;

        public UserManager(ContextEnum contextEnum)
        {
            repo = new UserRepository(contextEnum);
        }

        public User CreateUser(string email, string lastname, string firstname, string nickname, string imageUrl)
        {
            User user = new User
            {
                Email = email,
                LastName = lastname,
                FirstName = firstname,
                Nickname = nickname,
                ImageUrl = imageUrl
            };
            return repo.CreateUser(user);
        }

        public void DeleteUser(long userId)
        {
            repo.DeleteUser(userId);
        }

        public IEnumerable<UserRole> ReadOrganisationsForUser(long userId)
        {
            return repo.ReadOrganisationsForUser(userId);
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

        public IEnumerable<UserRole> ReadUserRolesForOrganisation(Organisation organisation)
        {
            return repo.ReadUserRolesForOrganisation(organisation);
        }

        public IEnumerable<User> ReadUsers()
        {
            return repo.ReadUsers();
        }

        public User UpdateUser(User user)
        {
            return repo.UpdateUser(user);
        }
    }
}
