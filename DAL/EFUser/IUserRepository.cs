using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.DAL.EFUser
{
    public interface IUserRepository
    {
        //User
        User CreateUser(User user);
        User UpdateUser(User user);
        User ReadUser(long userId);
        User ReadUser(string email);
        User ReadUser(string lastname, string firstname);
        User ReadOrganiserFromOrganisation(Organisation organisation);
        List<User> ReadUsers();
        void DeleteUser(long userId);

        //UserRole
        UserRole CreateUserRole(long userId, long organisationId, Role role);
        UserRole CreateUserRole(UserRole userRole);
        List<UserRole> ReadUserRolesForOrganisation(Organisation organisation);
        List<UserRole> ReadOrganisationsForUser(User user);
    }
}
