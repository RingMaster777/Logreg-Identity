using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LogReg_Identity.Models;
using LogReg_Identity.Models.ViewModel;
using LogReg_Identity.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LogReg_Identity.Controllers
{
    public class NoteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public NoteController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<NoteModel> notes = null;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));


            if (userRoles.Contains("Admin"))
            {
                notes = _unitOfWork.Note.GetAll().ToList();
            }
            else
            {
                notes = _unitOfWork.Note.GetAll().Where(u=> u.CreatorId == userId).ToList();
            }
            
            return View(notes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteVM noteVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var note = new NoteModel
                {
                    NoteTitle = noteVM.NoteTitle,
                    NoteDescription = noteVM.NoteDescription,
                    CreatedAt = DateTime.Now,
                    NoteAuthor = $"{user.FirstName} {user.LastName}",
                    CreatorId = user.Id
                };

                _unitOfWork.Note.Add(note);
                _unitOfWork.Save();

                TempData["successMessage"] = "A new note added successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["errorMessage"] = "Model state is invalid.";
            return View(noteVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var note = _unitOfWork.Note.Get(u => u.NoteId == id);
            if (note == null)
            {
                TempData["errorMessage"] = $"Note details not found with Id : {id}";
                return RedirectToAction(nameof(Index));
            }

            var noteVM = new NoteVM
            {
                NoteId = note.NoteId,
                NoteTitle = note.NoteTitle,
                NoteDescription = note.NoteDescription
            };

            return View(noteVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NoteVM noteVM)
        {
            if (ModelState.IsValid)
            {
                var existingNote = _unitOfWork.Note.Get(u => u.NoteId == noteVM.NoteId);
                if (existingNote == null)
                {
                    return NotFound(); // Handle the case where the entity is not found
                }

                existingNote.NoteTitle = noteVM.NoteTitle;
                existingNote.NoteDescription = noteVM.NoteDescription;

                _unitOfWork.Note.Update(existingNote);
                _unitOfWork.Save();

                TempData["successMessage"] = "Note updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["errorMessage"] = "Model state is invalid.";
            return View(noteVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int noteId)
        {
            try
            {
                var note = _unitOfWork.Note.Get(u => u.NoteId == noteId);
                if (note == null)
                {
                    TempData["errorMessage"] = "Note not found.";
                    return RedirectToAction(nameof(Index));
                }

                _unitOfWork.Note.Remove(note);
                _unitOfWork.Save();

                TempData["successMessage"] = "Note deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
