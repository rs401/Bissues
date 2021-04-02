using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bissues.Data;
using Bissues.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Ganss.XSS;
using Microsoft.Extensions.Logging;

namespace Bissues.Controllers
{
    [Authorize(Roles = "Admin,Developer")]
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<NoteController> _logger;
        public NoteController(ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<NoteController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Note
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OfficialNotes.Include(o => o.AppUser).Include(o => o.Bissue);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Note/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officialNote = await _context.OfficialNotes
                .Include(o => o.AppUser)
                .Include(o => o.Bissue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (officialNote == null)
            {
                return NotFound();
            }

            return View(officialNote);
        }

        // GET: Note/Create
        public IActionResult Create(int? bid)
        {
            if(bid != null)
            {
                ViewData["BissueId"] = new SelectList(_context.Bissues.Where(b => b.Id == bid).ToList(), "Id", "Description");
            }
            else
            {
                ViewData["BissueId"] = new SelectList(_context.Bissues, "Id", "Description");
            }
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BissueId,CommitId,CommitURL,Note,CreatedDate,ModifiedDate,AppUserId")] OfficialNote officialNote)
        {
            if (ModelState.IsValid)
            {
                officialNote.CreatedDate = DateTime.UtcNow;
                officialNote.ModifiedDate = DateTime.UtcNow;
                officialNote.AppUser = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Sanitize html if any
                string sanitizedNote = SanitizeString(officialNote.Note);
                officialNote.Note = sanitizedNote;

                _context.Add(officialNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", officialNote.AppUserId);
            ViewData["BissueId"] = new SelectList(_context.Bissues, "Id", "Description", officialNote.BissueId);
            return View(officialNote);
        }

        private string SanitizeString(string str)
        {
            var sanitizer = new HtmlSanitizer();
            var original = str.ToString();
            var sanitized = sanitizer.Sanitize(str);
            if(original != sanitized)
            {
                _logger.LogWarning(AppLogEvents.Error, 
                    $"Sanitizer Detection, Original:\n{original}\n"
                    + $"Sanitized:\n{sanitized}");
            }
            return sanitized;
        }

        // GET: Note/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officialNote = await _context.OfficialNotes.FindAsync(id);
            if (officialNote == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", officialNote.AppUserId);
            ViewData["BissueId"] = new SelectList(_context.Bissues, "Id", "Description", officialNote.BissueId);
            return View(officialNote);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BissueId,CommitId,CommitURL,Note,CreatedDate,ModifiedDate,AppUserId")] OfficialNote officialNote)
        {
            if (id != officialNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    officialNote.ModifiedDate = DateTime.UtcNow;

                    // Sanitize html if any
                    string sanitizedNote = SanitizeString(officialNote.Note);
                    officialNote.Note = sanitizedNote;

                    _context.Update(officialNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficialNoteExists(officialNote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", officialNote.AppUserId);
            ViewData["BissueId"] = new SelectList(_context.Bissues, "Id", "Description", officialNote.BissueId);
            return View(officialNote);
        }

        // GET: Note/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officialNote = await _context.OfficialNotes
                .Include(o => o.AppUser)
                .Include(o => o.Bissue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (officialNote == null)
            {
                return NotFound();
            }

            return View(officialNote);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var officialNote = await _context.OfficialNotes.FindAsync(id);
            _context.OfficialNotes.Remove(officialNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficialNoteExists(int id)
        {
            return _context.OfficialNotes.Any(e => e.Id == id);
        }
    }
}
