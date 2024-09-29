using LogReg_Identity.Models;
using LogReg_Identity.Models.ViewModel;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogReg_Identity.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuModel>> GetAllMenusAsync();
        Task CreateMenuAsync(MenuVM menuVM);
        Task<MenuModel?> GetByIdAsync(int id);
        Task UpdateMenuAsync(MenuModel existing, MenuModel updated);
        Task DeleteMenuAsync(int id);
        Task<List<SelectListItem>> GetRoleSelectListAsync();
        Task<List<SelectListItem>> GetMenuSelectListAsync();
    }
}
