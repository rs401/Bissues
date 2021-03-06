using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Bissues.Models;
using Bissues.ViewModels;
using Bissues.Data;

namespace Bissues.Controllers
{
    [Authorize(Roles = "Admin")]
    /// <summary>
    /// Home controller handles Home requests
    /// </summary>
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;

        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = GetAdminAreaViewModel();
            return View(model);
        }
        public IActionResult Bissues()
        {
            var model = GetAdminAreaViewModel();
            return View(model);
        }
        public async Task<IActionResult> BissueDetails(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var bissue = await _context.Bissues.FindAsync(id);
            if (bissue == null)
            {
                return NotFound();
            }
            AdminBissueViewModel model = new AdminBissueViewModel();
            model.Bissue = bissue;
            model.Users = await _context.AppUsers.ToListAsync();
            return View(bissue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBissue(int id, [Bind("Id,Title,Description,IsOpen,AppUserId,AppUser,ProjectId,CreatedDate,ModifiedDate,Label")] Bissue bissue)
        {
            if (id != bissue.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    bissue.ModifiedDate = DateTime.UtcNow;
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
                return RedirectToAction(nameof(Bissues));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", bissue.ProjectId);
            return View(bissue);
        }

        private AdminAreaViewModel GetAdminAreaViewModel()
        {
            return new AdminAreaViewModel()
            {
                Roles = _context.AppRole.ToList(),
                Users = _context.AppUsers.ToList(),
                Bissues = _context.Bissues.ToList(),
                Projects = _context.Projects.ToList(),
                Messages = _context.Messages.ToList(),
                Notifications = _context.Notifications.ToList()
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool BissueExists(int id)
        {
            return _context.Bissues.Any(e => e.Id == id);
        }
    }
}
