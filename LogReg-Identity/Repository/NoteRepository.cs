using LogReg_Identity.Data;
using LogReg_Identity.Models;
using LogReg_Identity.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LogReg_Identity.Repository
{
    public class NoteRepository : Repository<NoteModel>, INoteRepository
    {
        private ApplicationDbContext _db;

        public NoteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(NoteModel existingNote)
        {
            //_db.Notes.Update(obj);

            // Update properties
            //_db.Entry(existingNote).CurrentValues.SetValues(note);
            _db.Notes.Update(existingNote);

            Console.WriteLine("Hello");
        }

    }
}
