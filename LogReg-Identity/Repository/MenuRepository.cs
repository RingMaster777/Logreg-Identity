using LogReg_Identity.Data;
using LogReg_Identity.Models;
using LogReg_Identity.Repository.IRepository;

namespace LogReg_Identity.Repository
{
    public class MenuRepository : Repository<MenuModel>, IMenuRepository
    {
        private ApplicationDbContext _db;

        public MenuRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(MenuModel existingeMenu, MenuModel menu)
        {
            // Update properties
            _db.Entry(existingeMenu).CurrentValues.SetValues(menu);
        }
    }
}
