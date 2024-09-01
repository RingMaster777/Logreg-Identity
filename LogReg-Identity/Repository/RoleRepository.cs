using LogReg_Identity.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace LogReg_Identity.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityRole> GetByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<IEnumerable<IdentityRole>> GetAllAsync()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task AddAsync(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
        }

        public async Task UpdateAsync(IdentityRole role)
        {
            await _roleManager.UpdateAsync(role);
        }

        public async Task DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }
    }
}
