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

        public MenuController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            IEnumerable<MenuModel> Menus = null;
            if (_signInManager.IsSignedIn(User))
            {
                Menus =  _unitOfWork.Menu.GetAll().ToList();
            }
            return View(Menus);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync(); // Fetch roles from the database
            List<SelectListItem>  list = (from p in roles
                                          select new SelectListItem
                                          {
                                              Text = p.Name,
                                              Value = p.Id
                                          }).ToList();
            ViewBag.Roles = list; // Assuming Role has Id and Name properties



            var menus = _unitOfWork.Menu.GetAll().ToList();
            List<SelectListItem> listofMenu = (from p in menus
                                               select new SelectListItem
                                               {
                                                   Text = p.MenuName,
                                                   Value = p.MenuId.ToString()
                                               }).ToList();

            listofMenu.Insert(0, new SelectListItem { Value = "0", Text = "None" });

            ViewBag.Menus = listofMenu;
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
                    menuModel.MenuParentId =Int32.Parse(obj.ParentName);


                    _unitOfWork.Menu.Add(menuModel);
                    _unitOfWork.Save();

                    foreach (var item in obj.AssignTo)
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
                    var roles = await _roleManager.Roles.ToListAsync();
                    ViewBag.Roles = new MultiSelectList(roles, "Id", "Name");

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
            var Menu =  _unitOfWork.Menu.Get(u=> u.MenuId == id);

            if (Menu != null)
            {
                return View(Menu);
            }

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

                    var existingMenu = _unitOfWork.Menu.Get(u=> u.MenuId == Menu.MenuId);
                    if (existingMenu == null)
                    {
                        return View(); // Handle the case where the entity is not found
                    }

                    // Update properties
                    _unitOfWork.Menu.Update(existingMenu, Menu);
                    _unitOfWork.Save();

                    TempData["successMessage"] = "Menu updated successfully.";
                    return RedirectToAction(nameof(Index));

                    // return RedirectToAction(nameof(Index));
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
                var Menu = _unitOfWork.Menu.Get(u => u.MenuId == MenuId);

                if (Menu == null)
                {
                    return View(Menu);
                }
                // To delete the file 
                _unitOfWork.Menu.Remove(Menu);
                _unitOfWork.Save();

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
