using BookShop.Application.Authorization;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;

namespace BookShop.Infrastructure.Authorization
{
    internal class PermissionChecker : IPermissionChecker
    {
        #region constructor

        private readonly ICurrentUser _currentUser;
        private readonly IPermissionRepository _permissionRepository;
        public PermissionChecker(ICurrentUser currentUser, IPermissionRepository permissionRepository)
        {
            _currentUser = currentUser;
            _permissionRepository = permissionRepository;
        }

        #endregion



        public async Task<bool> HasPermission(string permissionName)
        {
            if (_currentUser.Authenticated == false)
                return false;

            var userPermissions = await _permissionRepository.GetUserPermissions(_currentUser.Id.Value);
            return userPermissions.Any(a => a.Name == permissionName);
        }


        public async Task<bool> HasPermission(string[] permissionNames)
        {
            if (_currentUser.Authenticated == false)
                return false;

            var userPermissions = await _permissionRepository.GetUserPermissions(_currentUser.Id.Value);
            var userPermissionNames = userPermissions.Select(a => a.Name);
            bool hasAllPermissions = permissionNames
                .Except(userPermissionNames)
                .Any() == false;
            return hasAllPermissions;
        }






    }
}
