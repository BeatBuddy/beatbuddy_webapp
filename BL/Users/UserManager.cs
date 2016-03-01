using System.Collections.Generic;
using BB.BL.Domain.Users;
using BB.BL.Domain;
using BB.DAL.EFUser;
using BB.BL.Domain.Organisations;
using BB.DAL.EFOrganisation;

namespace BB.BL
{
    public class UserManager : IUserManager
    {
        private IUserRepository repo;
        private IOrganisationManager mgrOrganisation;


        public UserManager(ContextEnum contextEnum)
        {
            mgrOrganisation = new OrganisationManager(contextEnum);
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

        public UserRole CreateUserRole(long userId, long organisationId, Role role)
        {
            return repo.CreateUserRole(userId, organisationId, role);
        }

        public UserRole CreateUserRole(User user, Organisation organisation, Role role)
        {
            UserRole userRole = new UserRole()
            {
                User = user,
                Organisation = organisation,
                Role = role
            };
            return repo.CreateUserRole(userRole);
        }

        public void DeleteUser(long userId)
        {
            repo.DeleteUser(userId);
        }

        public IEnumerable<UserRole> ReadOrganisationsForUser(long userId)
        {
            return repo.ReadOrganisationsForUser(userId);
        }

        public User ReadOrganiserFromOrganisation(Organisation organisation)
        {
            return repo.ReadOrganiserFromOrganisation(organisation);
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

        public IEnumerable<User> ReadCoOrganiserFromOrganisation(Organisation organisation)
        {
            return repo.ReadCoOrganiserFromOrganisation(organisation);
        }
    }
}
