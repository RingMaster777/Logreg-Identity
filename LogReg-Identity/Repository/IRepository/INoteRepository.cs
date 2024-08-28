using LogReg_Identity.Models;


namespace LogReg_Identity.Repository.IRepository
{
    public interface INoteRepository : IRepository<NoteModel>
    {
        void Update(NoteModel obj);
        void Save();
    }
}
 