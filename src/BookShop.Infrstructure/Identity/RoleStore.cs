using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookShop.Infrastructure.Identity
{
    internal sealed class RoleStore : IRoleStore<Role>
                           

    {
        #region contructor

        private readonly ICurrentUser _currentUser;
        private readonly IRoleRepository _roleRepository;
        public RoleStore(IRoleRepository roleRepository, ICurrentUser currentUser)
        {
            _roleRepository = roleRepository;
            _currentUser = currentUser;
        }

        #endregion


        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.Add(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.Delete(role);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            
        }

        public async Task<Role?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _roleRepository.Get(roleId);
        }

        public async Task<Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _roleRepository.GetByNormalizedNameOrDefaultAsync(normalizedRoleName);
        }

        public Task<string?> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.Update(role);
            return IdentityResult.Success;
        }







    }
}
