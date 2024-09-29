using LogReg_Identity.Models;
using LogReg_Identity.Repository.IRepository;

using Microsoft.AspNetCore.Identity;

namespace LogReg_Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserDto>> GetUsersWithRolesAsync()
        {
            return await _userRepo.GetUsersWithRolesAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _userRepo.GetByIdAsync(id);
        }

        public async Task<List<string>> GetAllRoleNamesAsync()
        {
            return await _userRepo.GetAllRoleNamesAsync();
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AssignRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded) return false;

            var addResult = await _userManager.AddToRoleAsync(user, roleName);
            return addResult.Succeeded;
        }
    }
}
