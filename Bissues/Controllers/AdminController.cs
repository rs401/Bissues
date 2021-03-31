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
    /// Admin controller handles Admin requests
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
        /// <summary>
        /// Index action returns the main admin dashboard view with an 
        /// AdminAreaViewModel.
        /// </summary>
        /// <returns>ViewResult</returns>
        public IActionResult Index()
        {
            var model = GetAdminAreaViewModel();
            return View(model);
        }
        /// <summary>
        /// Bissues returns a view with an AdminAreaViewModel. A list of all 
        /// Bissues linking to an Admin level edit page for each.
        /// </summary>
        /// <returns>ViewResult</returns>
        public IActionResult Bissues()
        {
            var model = GetAdminAreaViewModel();
            model.Bissues = model.Bissues
                .Where(b => b.Label == BissueLabel.Issue)
                .OrderByDescending(b => b.CreatedDate)
                .ThenBy(b => b.IsOpen).ToList();
            return View(model);
        }
        /// <summary>
        /// Bugs returns a view with an AdminBugsViewModel. A list of all 
        /// Bissues that have been labeled as Bug, linking to an Admin level 
        /// edit page for each.
        /// </summary>
        /// <returns>ViewResult</returns>
        public IActionResult Bugs()
        {
            var model = new AdminBugsViewModel()
            {
                Bugs = _context.Bissues
                    .Include(b => b.Project)
                    .Where(b => b.Label == BissueLabel.Bug).ToList()
            };
            return View(model);
        }
        /// <summary>
        /// Users returns a view with a list of all users, with the ability to 
        /// lock and unlock an account.
        /// </summary>
        /// <returns>ViewResult</returns>
        public IActionResult Users()
        {
            var model = _context.AppUsers.ToList();
            return View(model);
        }
        /// <summary>
        /// LockUser takes a user id string and locks the account.
        /// </summary>
        /// <param name="sid">user id</param>
        /// <returns>Redirect to Users</returns>
        public async Task<IActionResult> ToggleLockUser(string sid)
        {
            if(string.IsNullOrEmpty(sid))
            {
                return NotFound();
            }
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == sid);
            if(user == null)
            {
                return NotFound();
            }
            
            if(user.LockoutEnabled == true)
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.Now.AddYears(2);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Users");
        }
        /// <summary>
        /// UnLockUser takes a user id string and unlocks the account.
        /// </summary>
        /// <param name="sid">user id</param>
        /// <returns>Redirect to Users</returns>
        public async Task<IActionResult> UnLockUser(string sid)
        {
            if(string.IsNullOrEmpty(sid))
            {
                return NotFound();
            }
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == sid);
            if(user == null)
            {
                return NotFound();
            }
            user.LockoutEnabled = false;
            user.LockoutEnd = null;
            await _context.SaveChangesAsync();
            return RedirectToAction("Users");
        }
        /// <summary>
        /// BissueDetails is the Admin level edit view.
        /// </summary>
        /// <param name="id">Bissue Id</param>
        /// <returns>ViewResult</returns>
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
            return View(bissue);
        }
        /// <summary>
        /// EditBissue POST takes an id and a Bissue model, and edits the Bissue
        /// </summary>
        /// <param name="id">Bissue Id</param>
        /// <param name="bissue">Bissue Model</param>
        /// <returns>Redirect to Bissues</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBissue(int id, [Bind("Id,Title,Description,IsOpen,AppUserId,AppUser,ProjectId,CreatedDate,ModifiedDate,Label,AssignedDeveloperId")] Bissue bissue)
        {
            if (id != bissue.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // If closed and null closed date, set closed date
                    if(bissue.IsOpen == false && bissue.ClosedDate == null)
                    {
                        bissue.ClosedDate = DateTime.UtcNow;
                    }
                    bissue.ModifiedDate = DateTime.UtcNow;
                    _context.Update(bissue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException dbe)
                {
                    // If Bissue has been deleted
                    if (!BissueExists(bissue.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(dbe, "Admin Edit POST DbUpdateConcurrencyException");
                        throw;
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(AppLogEvents.Update, ex, "Admin Edit POST DbUpdateConcurrencyException");
                    throw;
                }
                return RedirectToAction(nameof(Bissues));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", bissue.ProjectId);
            return View(bissue);
        }
        /// <summary>
        /// GetAdminAreaViewModel constructs and returns a AdminAreaViewModel.
        /// </summary>
        /// <returns>AdminAreaViewModel</returns>
        private AdminAreaViewModel GetAdminAreaViewModel()
        {
            return new AdminAreaViewModel()
            {
                Bissues = _context.Bissues.ToList(),
                Projects = _context.Projects.ToList()
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
