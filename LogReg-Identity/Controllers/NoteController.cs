using LogReg_Identity.Data;
using LogReg_Identity.Models;
using LogReg_Identity.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogReg_Identity.Controllers
{
    public class NoteController : Controller
    {

        private readonly INoteRepository _noteRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public NoteController(INoteRepository noteRepository, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _noteRepository = noteRepository;

        }
        public IActionResult Index()
        {
            IEnumerable<NoteModel> notes = null;
            if (_signInManager.IsSignedIn(User))
            {
                notes = _noteRepository.GetAll().ToList();
            }
            return View(notes);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // To Create a new note data
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoteModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _noteRepository.Add(obj);
                    _noteRepository.Save();

                    TempData["successMessage"] = "A new Note added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["errorMessage"] = "Model State is invalid";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }



        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var note = _noteRepository.Get(u => u.NoteId == id);

            if (note != null)
            {
                return View(note);
            }

            TempData["errorMessage"] = $"note details not found with Id : {id}";

            return RedirectToAction(nameof(Index));
        }



        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoteModel note)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingNote = _noteRepository.Get(u => u.NoteId == note.NoteId);
                    if (existingNote == null)
                    {
                        return View(); // Handle the case where the entity is not found
                    }

                    // Update properties
                    _noteRepository.Update(note);
                    _noteRepository.Save();

                    TempData["successMessage"] = "note updated successfully.";
                    return RedirectToAction(nameof(Index));
                    // return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["errorMessage"] = "Model State is invalid";
                    return View(note);
                }
            }
            catch (System.Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public  IActionResult Delete(int noteId)
        {
            try
            {
                var note = _noteRepository.Get(u => u.NoteId == noteId);
                if (note == null)
                {
                    return View(note);
                }
                // To delete the file 
                _noteRepository.Remove(note);
                _noteRepository.Save();

                TempData["successMessage"] = "note deleted successfully.";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }


        }


    }
}
