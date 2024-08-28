using LogReg_Identity.Data;
using LogReg_Identity.Models;
using LogReg_Identity.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LogReg_Identity.Controllers
{
    public class MenuController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public MenuController(ApplicationDbContext db, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _db = db;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<MenuModel> Menus = null;
            if (_signInManager.IsSignedIn(User))
            {

                Menus = await _db.Menus.ToListAsync();

            }
            return View(Menus);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _db.Roles.ToListAsync(); // Fetch roles from the database
            List<SelectListItem>  list = (from p in roles
                                          select new SelectListItem
                                          {
                                              Text = p.Name,
                                              Value = p.Id
                                          }).ToList();
            ViewBag.Roles = list; // Assuming Role has Id and Name properties



            var menus =  await _db.Menus.ToListAsync();
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
                    //Console.WriteLine(obj.AssignTo);
                    _db.Menus.Add(menuModel);
                    await _db.SaveChangesAsync();




                    foreach (var item in obj.AssignTo)
                    {
                        MenuPermissionModel menuPermissionModel = new MenuPermissionModel();
                        menuPermissionModel.MenuId = menuModel.MenuId;
                        menuPermissionModel.RoleId = item;
                        _db.MenuPermissions.Add(menuPermissionModel);
                        await _db.SaveChangesAsync();
                    }

                    

                    TempData["successMessage"] = "A new Menu added Successfully";

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["errorMessage"] = "Model State is invalid";
                    // If the model state is invalid, repopulate the roles dropdown
                    var roles = await _db.Roles.ToListAsync();
                    ViewBag.Roles = new MultiSelectList(roles, "Id", "Name");

                    return View(obj);
                }
            }
            catch (System.Exception ex)
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

            var Menu = await _db.Menus.FindAsync(id);

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




                    var existingMenu = await _db.Menus.FindAsync(Menu.MenuId);
                    if (existingMenu == null)
                    {
                        return View(); // Handle the case where the entity is not found
                    }

                    // Update properties
                    _db.Entry(existingMenu).CurrentValues.SetValues(Menu);

                    await _db.SaveChangesAsync();

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
            catch (System.Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();

            }


        }

        [HttpPost]
        public async Task<IActionResult> Delete(int MenuId)
        {
            var Menu = await _db.Menus.FindAsync(MenuId);

            if (Menu == null)
            {
                return View(Menu);
            }
            // To delete the file 
            var result = _db.Menus.Remove(Menu);

            await _db.SaveChangesAsync();

            TempData["successMessage"] = "Menu deleted successfully.";
            return RedirectToAction(nameof(Index));
        }


    }
}
