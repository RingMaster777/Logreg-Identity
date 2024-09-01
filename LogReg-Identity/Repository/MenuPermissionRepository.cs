using LogReg_Identity.Data;
using LogReg_Identity.Models;
using LogReg_Identity.Repository.IRepository;

namespace LogReg_Identity.Repository
{
    public class MenuPermissionRepository : Repository<MenuPermissionModel>, IMenuPermissionRepository
    {
        private ApplicationDbContext _db;

        public MenuPermissionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(MenuPermissionModel existingMenu, MenuPermissionModel menu)
        {
            throw new NotImplementedException();
        }

    }
}
