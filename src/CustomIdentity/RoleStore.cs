using Microsoft.AspNet.Identity;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using DAL.Interfaces;

namespace CustomIdentity
{
    public class RoleStore : IRoleStore<IdentityRole>
    {
        IUOW _uow;

        public RoleStore(IUOW uow)
        {
            _uow = uow;
        }

        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // TODO: should this be empty?
        }

        public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return new IdentityRole { Name = normalizedRoleName };
        }

        public async Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return role.Name.ToLower();
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return role.Name;
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
