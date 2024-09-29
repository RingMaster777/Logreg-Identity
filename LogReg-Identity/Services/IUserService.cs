using LogReg_Identity.Models;

namespace LogReg_Identity.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersWithRolesAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<List<string>> GetAllRoleNamesAsync();
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> AssignRoleAsync(string userId, string roleName);
    }
}
