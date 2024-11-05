using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Authorization
{
    public interface IPermissionChecker
    {

        public Task<bool> HasPermission(string permissionName);

        public Task<bool> HasPermission(string[] permissionNames);


    }




    
}
