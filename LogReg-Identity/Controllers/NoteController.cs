using LogReg_Identity.Areas.Identity.Data;
using LogReg_Identity.Data;
using LogReg_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LogReg_Identity.Controllers
{
    public class NoteController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public NoteController(ApplicationDbContext db, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _db = db;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<NoteModel> notes = null;
            if (_signInManager.IsSignedIn(User))
            {

                notes = await _db.Notes.ToListAsync();

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
        public async Task<IActionResult> Create(NoteModel obj)
        {

            try 
            { 
           
            //    bool exists = await _db.Notes.AnyAsync(e => e.ErpCifNo == erpCifNo); ;

            //    if (exists)
            //    {
            //        TempData["errorMessage"] = "This Erp No is already occupied by an note ";
            //        ModelState.Clear();
            //        return View();

            //    }


                if (ModelState.IsValid)
                {
                    _db.Notes.Add(obj);
                    await _db.SaveChangesAsync();
                    
                    TempData["successMessage"] = "A new Note added Successfully";

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["errorMessage"] = "Model State is invalid";
                    return View();
                }
            }
            catch (System.Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var note = await _db.Notes.FindAsync(id);

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
        public async Task<IActionResult> Edit(NoteModel note)
        {

            if (ModelState.IsValid)
            {

                try
                {


                    var existingNote = await _db.Notes.FindAsync(note.NoteId);
                    if (existingNote == null)
                    {
                        return View(); // Handle the case where the entity is not found
                    }

                    // Update properties
                    _db.Entry(existingNote).CurrentValues.SetValues(note);

                    await _db.SaveChangesAsync();
                    
                    TempData["successMessage"] = "note updated successfully.";
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                    return View();

                }
                // return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["errorMessage"] = "Model State is invalid";
                return View(note);
            }


        }

        [HttpPost]
        public async Task<IActionResult> Delete(int noteId)
        {
            var note = await _db.Notes.FindAsync(noteId);

            if (note == null)
            {
                return View(note);
            }
            // To delete the file 
            var result = _db.Notes.Remove(note);

            await _db.SaveChangesAsync();
            
            TempData["successMessage"] = "note deleted successfully.";
            return RedirectToAction(nameof(Index));
        }


    }
}
