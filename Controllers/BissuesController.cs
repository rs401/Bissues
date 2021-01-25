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
    public class BissuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BissuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bissues
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bissues.Include(b => b.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bissues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bissue = await _context.Bissues
                .Include(b => b.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bissue == null)
            {
                return NotFound();
            }

            return View(bissue);
        }

        // GET: Bissues/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: Bissues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,AppUserId,ProjectId,CreatedDate,ModifiedDate")] Bissue bissue)
        {
            if (ModelState.IsValid)
            {
                /* Need to set AppUserId to current user */
                _context.Add(bissue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", bissue.ProjectId);
            return View(bissue);
        }

        // GET: Bissues/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bissue = await _context.Bissues.FindAsync(id);
            if (bissue == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", bissue.ProjectId);
            return View(bissue);
        }

        // POST: Bissues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,AppUserId,ProjectId,CreatedDate,ModifiedDate")] Bissue bissue)
        {
            if (id != bissue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bissue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BissueExists(bissue.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", bissue.ProjectId);
            return View(bissue);
        }

        // GET: Bissues/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bissue = await _context.Bissues
                .Include(b => b.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bissue == null)
            {
                return NotFound();
            }

            return View(bissue);
        }

        // POST: Bissues/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bissue = await _context.Bissues.FindAsync(id);
            _context.Bissues.Remove(bissue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BissueExists(int id)
        {
            return _context.Bissues.Any(e => e.Id == id);
        }
    }
}
