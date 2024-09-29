using LogReg_Identity.Models;
namespace LogReg_Identity.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetUsersWithRolesAsync();
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<List<string>> GetAllRoleNamesAsync();
    }
}
