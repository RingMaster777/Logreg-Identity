using LogReg_Identity.Data;
using LogReg_Identity.Models;
using LogReg_Identity.Repository.IRepository;

namespace LogReg_Identity.Repository
{
    public class NoteRepository : Repository<NoteModel>, INoteRepository
    {
        private ApplicationDbContext _db;

        public NoteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(NoteModel obj)
        {
            _db.Notes.Update(obj);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
