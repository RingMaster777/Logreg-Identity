using LogReg_Identity.Data;
using LogReg_Identity.Models;
using LogReg_Identity.Models.ViewModel;
using LogReg_Identity.Repository;
using LogReg_Identity.Repository.IRepository;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LogReg_Identity.Controllers
{
    public class MenuController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LogReg_Identity.Services.IMenuService _menuService;

        public MenuController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, LogReg_Identity.Services.IMenuService menuService)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _menuService = menuService;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<MenuModel> Menus = Enumerable.Empty<MenuModel>();
            if (_signInManager.IsSignedIn(User))
            {
                Menus = await _menuService.GetAllMenusAsync();
            }
            return View(Menus);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _menuService.GetRoleSelectListAsync();
            ViewBag.Menus = await _menuService.GetMenuSelectListAsync();
            return View();
        }


        // To Create a new Menu data
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MenuModel menuModel = new MenuModel();

                    menuModel.MenuName = obj.MenuName;
                    menuModel.MenuParentId = Int32.TryParse(obj.ParentName, out var parentId) ? parentId : 0;


                    _unitOfWork.Menu.Add(menuModel);
                    _unitOfWork.Save();

                    foreach (var item in obj.AssignTo ?? Enumerable.Empty<string>())
                    {
                        MenuPermissionModel menuPermissionModel = new MenuPermissionModel();
                        menuPermissionModel.MenuId = menuModel.MenuId;
                        menuPermissionModel.RoleId = item;
                        _unitOfWork.MenuPermission.Add(menuPermissionModel);
                        _unitOfWork.Save();
                    }
                    TempData["successMessage"] = "A new Menu added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["errorMessage"] = "Model State is invalid";
                    ViewBag.Roles = await _menuService.GetRoleSelectListAsync();
                    return View(obj);
                }
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var Menu = await _menuService.GetByIdAsync(id.Value);
            if (Menu != null) return View(Menu);
            TempData["errorMessage"] = $"Menu details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }



        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuModel Menu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMenu = await _menuService.GetByIdAsync(Menu.MenuId);
                    if (existingMenu == null) return View();
                    await _menuService.UpdateMenuAsync(existingMenu, Menu);
                    TempData["successMessage"] = "Menu updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["errorMessage"] = "Model State is invalid";
                    return View(Menu);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int MenuId)
        {
            try
            {
                await _menuService.DeleteMenuAsync(MenuId);
                TempData["successMessage"] = "Menu deleted successfully.";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
    }
}
