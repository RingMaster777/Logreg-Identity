using LogReg_Identity.Data;
using LogReg_Identity.Repository.IRepository;

namespace LogReg_Identity.Repository
{
    public class UnitOfWork : IUnitOfWork
    {


        private ApplicationDbContext _db;
        public INoteRepository Note { get; private set; }

        public IMenuRepository Menu { get; private set; }

        public IMenuPermissionRepository MenuPermission { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Note =  new NoteRepository(_db);
            Menu = new MenuRepository(_db);
            MenuPermission = new MenuPermissionRepository(_db);
        }
        
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
