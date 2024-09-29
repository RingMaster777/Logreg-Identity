using LogReg_Identity.Models;
using LogReg_Identity.Models.ViewModel;

namespace LogReg_Identity.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteModel>> GetNotesForUserAsync(ApplicationUser user, IList<string> roles);
        Task CreateNoteAsync(NoteVM noteVM, ApplicationUser user);
        Task<NoteModel?> GetByIdAsync(int id);
        Task UpdateNoteAsync(NoteModel note);
        Task DeleteNoteAsync(int id);
    }
}
