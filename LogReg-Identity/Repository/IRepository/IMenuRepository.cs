using LogReg_Identity.Models;

namespace LogReg_Identity.Repository.IRepository
{
    public interface IMenuRepository : IRepository<MenuModel>
    {
        void Update(MenuModel existingMenu, MenuModel menu);
    }
}
