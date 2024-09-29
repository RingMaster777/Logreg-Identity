using LogReg_Identity.Data;
using LogReg_Identity.Models;
using LogReg_Identity.Repository.IRepository;

using Microsoft.EntityFrameworkCore;

namespace LogReg_Identity.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserDto>> GetUsersWithRolesAsync()
        {
            var list = await (from u in _db.Users
                              join ur in _db.Set<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>() on u.Id equals ur.UserId into urj
                              from ur in urj.DefaultIfEmpty()
                              join r in _db.Roles on ur.RoleId equals r.Id into rj
                              from r in rj.DefaultIfEmpty()
                              select new UserDto
                              {
                                  Id = u.Id,
                                  FirstName = u.FirstName,
                                  LastName = u.LastName,
                                  Email = u.Email,
                                  Role = r != null ? r.Name : null
                              }).ToListAsync();

            return list;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<string>> GetAllRoleNamesAsync()
        {
            return await _db.Roles.Select(r => r.Name ?? string.Empty).ToListAsync();
        }
    }
}
