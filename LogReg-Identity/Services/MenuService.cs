using LogReg_Identity.Models;
using LogReg_Identity.Models.ViewModel;
using LogReg_Identity.Repository.IRepository;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LogReg_Identity.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _uow;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MenuService(IUnitOfWork uow, RoleManager<IdentityRole> roleManager)
        {
            _uow = uow;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<MenuModel>> GetAllMenusAsync()
        {
            return _uow.Menu.GetAll().ToList();
        }

        public async Task CreateMenuAsync(MenuVM menuVM)
        {
            if (menuVM == null) throw new ArgumentNullException(nameof(menuVM));
            if (string.IsNullOrWhiteSpace(menuVM.MenuName)) throw new ArgumentException("Menu name is required", nameof(menuVM));

            var parentId = int.TryParse(menuVM.ParentName, out var pid) ? pid : 0;

            var menuModel = new MenuModel
            {
                MenuName = menuVM.MenuName,
                MenuParentId = parentId
            };

            _uow.Menu.Add(menuModel);
            _uow.Save();

            var assignTo = menuVM.AssignTo ?? new List<string>();
            foreach (var item in assignTo)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                var menuPermissionModel = new MenuPermissionModel
                {
                    MenuId = menuModel.MenuId,
                    RoleId = item
                };
                _uow.MenuPermission.Add(menuPermissionModel);
                _uow.Save();
            }
        }

        public async Task<MenuModel?> GetByIdAsync(int id)
        {
            return _uow.Menu.Get(m => m.MenuId == id);
        }

        public async Task UpdateMenuAsync(MenuModel existing, MenuModel updated)
        {
            _uow.Menu.Update(existing, updated);
            _uow.Save();
        }

        public async Task DeleteMenuAsync(int id)
        {
            var menu = _uow.Menu.Get(m => m.MenuId == id);
            if (menu != null)
            {
                _uow.Menu.Remove(menu);
                _uow.Save();
            }
        }

        public async Task<List<SelectListItem>> GetRoleSelectListAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Id }).ToList();
        }

        public async Task<List<SelectListItem>> GetMenuSelectListAsync()
        {
            var menus = _uow.Menu.GetAll().ToList();
            var list = menus.Select(m => new SelectListItem { Text = m.MenuName, Value = m.MenuId.ToString() }).ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "None" });
            return list;
        }
    }
}
