using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System.Collections.Generic;

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
        IEnumerable<User> ReadUsers();
        User ReadOrganiserFromOrganisation(Organisation organisation);
        IEnumerable<User> ReadCoOrganiserFromOrganisation(Organisation organisation);
        void DeleteUser(User user);

        //UserRole
        IEnumerable<UserRole> ReadUserRolesForOrganisation(Organisation organisation);
        IEnumerable<UserRole> ReadOrganisationsForUser(long userId);
        UserRole CreateUserRole(long userId, long organisationId, Role role);
        UserRole CreateUserRole(UserRole userRole);
        UserRole ReadUserRoleForUserAndOrganisation(long userId, long organisationId);
        void DeleteUserRole(UserRole userRole);
    }
}
