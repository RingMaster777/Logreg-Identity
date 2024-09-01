using Microsoft.AspNetCore.Identity;

namespace LogReg_Identity.Repository.IRepository
{
    public interface IRoleRepository
    {
        Task<IdentityRole> GetByIdAsync(string id);
        Task<IEnumerable<IdentityRole>> GetAllAsync();
        Task AddAsync(IdentityRole role);
        Task UpdateAsync(IdentityRole role);
        Task DeleteAsync(string id);
    }
}
