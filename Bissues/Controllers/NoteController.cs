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

namespace Bissues.Controllers
{
    [Authorize(Roles = "Admin,Developer")]
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoteController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["BissueId"] = new SelectList(_context.Bissues, "Id", "Description");
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
                _context.Add(officialNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", officialNote.AppUserId);
            ViewData["BissueId"] = new SelectList(_context.Bissues, "Id", "Description", officialNote.BissueId);
            return View(officialNote);
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
