using LogReg_Identity.Models;
using LogReg_Identity.Models.ViewModel;
using LogReg_Identity.Repository.IRepository;

using Microsoft.AspNetCore.Identity;

namespace LogReg_Identity.Services
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;

        public NoteService(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<IEnumerable<NoteModel>> GetNotesForUserAsync(ApplicationUser user, IList<string> roles)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (roles == null) roles = new List<string>();

            if (roles.Contains("Admin"))
            {
                return _uow.Note.GetAll().ToList();
            }

            return _uow.Note.GetAll().Where(n => n.CreatorId == user.Id).ToList();
        }

        public async Task CreateNoteAsync(NoteVM noteVM, ApplicationUser user)
        {
            if (noteVM == null) throw new ArgumentNullException(nameof(noteVM));
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(noteVM.NoteTitle)) throw new ArgumentException("Note title is required", nameof(noteVM));
            if (string.IsNullOrWhiteSpace(noteVM.NoteDescription)) throw new ArgumentException("Note description is required", nameof(noteVM));

            var author = $"{user.FirstName ?? string.Empty} {user.LastName ?? string.Empty}".Trim();
            if (string.IsNullOrEmpty(author)) author = user.Email ?? "Unknown";

            if (string.IsNullOrWhiteSpace(user.Id)) throw new ArgumentException("User Id is required", nameof(user));

            var note = new NoteModel
            {
                NoteTitle = noteVM.NoteTitle,
                NoteDescription = noteVM.NoteDescription,
                CreatedAt = DateTime.Now,
                NoteAuthor = author,
                CreatorId = user.Id
            };

            _uow.Note.Add(note);
            _uow.Save();
        }

        public async Task<NoteModel?> GetByIdAsync(int id)
        {
            return _uow.Note.Get(n => n.NoteId == id);
        }

        public async Task UpdateNoteAsync(NoteModel note)
        {
            _uow.Note.Update(note);
            _uow.Save();
        }

        public async Task DeleteNoteAsync(int id)
        {
            var note = _uow.Note.Get(n => n.NoteId == id);
            if (note != null)
            {
                _uow.Note.Remove(note);
                _uow.Save();
            }
        }
    }
}
