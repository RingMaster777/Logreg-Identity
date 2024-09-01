using LogReg_Identity.Models;

namespace LogReg_Identity.Repository.IRepository
{
    public interface IMenuPermissionRepository : IRepository<MenuPermissionModel>
    {
        void Update(MenuPermissionModel existingMenuPermission, MenuPermissionModel menuPermission);
    }
}

