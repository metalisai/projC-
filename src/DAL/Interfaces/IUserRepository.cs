using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        IList<User> All { get; }
        IList<LendObject> GetUserObjects(string userId);
        User FindByUserName(string username);
        void AddUserToRole(User user, string roleName);
        void RemoveUserFromRole(User user, string roleName);
        IList<string> GetUserRoles(User user);
        IList<IdentityUserClaim> GetUserClaims(User user);
        void AddUserClaim(User user, IdentityUserClaim identityUserClaim);
        IEnumerable<IdentityUserClaim> UpdateUserClaims(User user, IdentityUserClaim oldclaim, IdentityUserClaim newclaim);
        void RemoveUserClaims(User user, IdentityUserClaim oldclaim);
        void AddUserLogin(User user, IdentityUserLogin l);
        void RemoveUserLogin(User user, string loginProvider, string providerKey);
        IList<IdentityUserLogin> GetUserLogins(User user);
        User GetUserByLogin(string loginProvider, string providerKey);
        User GetUserByEmail(string email);
        IList<User> GetUsersForClaim(IdentityUserClaim identityUserClaim);
        void AddUser(User user);
        void RemoveUser(User user);
        void SetUserField(User user, string fieldName, object value);
        User FindById(string userId);
    }
}
