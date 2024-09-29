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

// this controller is created to handle all the notes available in the system

namespace LogReg_Identity.Controllers
{
    public class NoteController : Controller
    {
        private readonly LogReg_Identity.Services.INoteService _noteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NoteController(LogReg_Identity.Services.INoteService noteService, UserManager<ApplicationUser> userManager)
        {
            _noteService = noteService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<NoteModel> notes = Enumerable.Empty<NoteModel>();
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            notes = await _noteService.GetNotesForUserAsync(user, userRoles);

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
                if (user == null)
                {
                    TempData["errorMessage"] = "Unable to resolve current user.";
                    return RedirectToAction("Index");
                }

                await _noteService.CreateNoteAsync(noteVM, user);
                TempData["successMessage"] = "A new note added successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["errorMessage"] = "Model state is invalid.";
            return View(noteVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var note = await _noteService.GetByIdAsync(id.Value);
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
                var existingNote = await _noteService.GetByIdAsync(noteVM.NoteId);
                if (existingNote == null) return NotFound();
                existingNote.NoteTitle = noteVM.NoteTitle;
                existingNote.NoteDescription = noteVM.NoteDescription;
                await _noteService.UpdateNoteAsync(existingNote);

                TempData["successMessage"] = "Note updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["errorMessage"] = "Model state is invalid.";
            return View(noteVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int noteId)
        {
            try
            {
                await _noteService.DeleteNoteAsync(noteId);
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
