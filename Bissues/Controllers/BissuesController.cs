using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bissues.Data;
using Bissues.Models;
using Bissues.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Westwind.AspNetCore.Markdown;
using Ganss.XSS;

namespace Bissues.Controllers
{
    /// <summary>
    /// Bissues controller handles Bissue requests
    /// </summary>
    public class BissuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BissuesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bissues
        /// <summary>
        /// Bissues Index
        /// </summary>
        /// <returns>Returns Bissues Index view populated with a list of all Bissues</returns>
        public IActionResult Index(int? index)
        {
            // var bissues = await _context.Bissues.Include(b => b.Project).OrderByDescending(b => b.IsOpen).ThenByDescending(b => b.ModifiedDate).ToListAsync();
            if(index == null)
            {
                return View(GetBissues(1));
            }
            return View(GetBissues((int)index));
        }
        /// <summary>
        /// Search takes an index for pagination and a query and returns a view 
        /// with the Bissues who's Title or Description contain the query.
        /// </summary>
        /// <param name="index">index for pagination</param>
        /// <param name="query">Query string to search for</param>
        /// <returns>ViewResult</returns>
        public IActionResult Search(int? index, string query)
        {
            // var bissues = await _context.Bissues.Include(b => b.Project).OrderByDescending(b => b.IsOpen).ThenByDescending(b => b.ModifiedDate).ToListAsync();
            if(index == null)
            {
                if(query == null)
                {
                    return View(GetBissues(1));
                }
                else
                {
                    ViewBag.query = query;
                    return View(GetSearchBissues(query, 1));
                }
            }
            if(query == null)
            {
                return View(GetBissues((int)index));
            }
            return View(GetSearchBissues(query, (int)index));
        }
        /// <summary>
        /// Returns a BissuesIndexViewModel with a list of Bissues that contain 
        /// the query.
        /// </summary>
        /// <param name="query">Query string we are looking for</param>
        /// <param name="index">index for pagination</param>
        /// <returns>BissuesIndexViewModel</returns>
        private BissuesIndexViewModel GetSearchBissues(string query, int index)
        {
            int maxRows = 10;
            BissuesIndexViewModel bivModel = new BissuesIndexViewModel();
            var bissues = _context.Bissues.Include(b => b.Project)
                .Where(b => b.Description.ToLower().Contains(query.ToLower()) || b.Title.ToLower().Contains(query.ToLower()))
                .OrderByDescending(b => b.IsOpen).ThenByDescending(b => b.ModifiedDate).ToList();
            bivModel.Bissues = bissues.Skip((index - 1) * maxRows).Take(maxRows).ToList();
            bivModel.CurrentIndex = index;
            bivModel.PageCount = (bissues.Count / maxRows) + 1;
            return bivModel;
        }
        /// <summary>
        /// Constructs and returns a BissuesIndexViewModel
        /// </summary>
        /// <param name="index">index for pagination</param>
        /// <returns>BissuesIndexViewModel</returns>
        private BissuesIndexViewModel GetBissues(int index)
        {
            int maxRows = 10;
            BissuesIndexViewModel bivModel = new BissuesIndexViewModel();
            var bissues = _context.Bissues.Include(b => b.Project).OrderByDescending(b => b.IsOpen).ThenByDescending(b => b.ModifiedDate).ToList();
            bivModel.Bissues = bissues.Skip((index - 1) * maxRows).Take(maxRows).ToList();
            bivModel.CurrentIndex = index;
            bivModel.PageCount = (bissues.Count / maxRows) + 1;
            return bivModel;
        }

        // GET: Bissues/Details/5
        /// <summary>
        /// Bissues Details view lists the details of the Bissue as well as all 
        /// Messages related to the Bissue
        /// </summary>
        /// <param name="id">Bissue Id</param>
        /// <returns>Details view lists the details of the Bissue as well as all 
        /// Messages related to the Bissue</returns>
        public async Task<IActionResult> Details(int? id, int? currentIndex)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bissue = await _context.Bissues.Include(b => b.MeToos).FirstOrDefaultAsync(b => b.Id == id);
            if (bissue == null)
            {
                return NotFound();
            }
            if(bissue.MeToos == null)
            {
                bissue.MeToos = new List<MeToo>();
                _context.Update(bissue);
                _context.SaveChanges();
            }

            if(currentIndex == null)
            {
                return View(GetDetailsViewModel((int)id,1));
            }
            return View(GetDetailsViewModel((int)id,(int)currentIndex));
        }
        /// <summary>
        /// AddMeToo increments the Bissues MeToo if the IP is unique
        /// </summary>
        /// <param name="id">Bissue Id</param>
        /// <returns>Redirect to Bissue Details</returns>
        public async Task<IActionResult> AddMeToo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bissue = await _context.Bissues.Include(b => b.MeToos).FirstOrDefaultAsync(b => b.Id == id);
            if (bissue == null)
            {
                return NotFound();
            }
            var theIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var unique = _context.MeToos.Where(mt => mt.Ip == theIp && mt.Bissue == bissue).ToList();
            if(unique.Count != 0)
            {
                return RedirectToAction(nameof(Details), new {id});
            }
            var meToo = new MeToo
            {
                Ip = theIp,
                Bissue = bissue
            };
            try
            {
                _context.MeToos.Add(meToo);
                await _context.SaveChangesAsync();
                _context.Update(bissue);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Details), new {id});
        }
        /// <summary>
        /// Constructs and returns a BissuesDetailsViewModel
        /// </summary>
        /// <param name="id">Bissue Id</param>
        /// <param name="index">index for pagination</param>
        /// <returns>BissuesDetailsViewModel</returns>
        private BissuesDetailsViewModel GetDetailsViewModel(int id, int index)
        {
            int maxRows = 10;
            BissuesDetailsViewModel bdvModel = new BissuesDetailsViewModel();
            var bissue = _context.Bissues
                .Include(b => b.Project)
                .Include(b => b.AppUser)
                .FirstOrDefault(b => b.Id == id);
            bdvModel.Bissue = bissue;
            var messages = _context.Messages.Where(m => m.BissueId == id).Include(m => m.AppUser).OrderBy(m => m.CreatedDate).ToList();
            bdvModel.Messages = messages.Skip((index - 1) * maxRows).Take(maxRows).ToList();
            bdvModel.CurrentIndex = index;
            bdvModel.PageCount = (messages.Count / maxRows) + 1;
            return bdvModel;
        }

        // GET: Bissues/Create
        [Authorize]
        /// <summary>
        /// Bissues Create GET view displays a form to create a Bissue
        /// </summary>
        /// <returns>Bissues Create view displays a form to create a Bissue</returns>
        public IActionResult Create(int? pid)
        {
            if(pid != null)
            {
                ViewData["ProjectId"] = new SelectList(_context.Projects.Where(p => p.Id == pid).ToList(), "Id", "Name");
            }
            else
            {
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            }
            return View();
        }

        // POST: Bissues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Bissues Create POST takes a Bissue and saves the Bissue to the db 
        /// and sets the created/modified date for the bissue as well as alters 
        /// the Modified date of the related Project.
        /// </summary>
        /// <param name="bissue"></param>
        /// <returns></returns>
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsOpen,AppUserId,AppUser,ProjectId,CreatedDate,ModifiedDate")] Bissue bissue)
        {
            if (ModelState.IsValid)
            {
                bissue.CreatedDate = DateTime.UtcNow;
                bissue.ModifiedDate = DateTime.UtcNow;
                bissue.AppUser = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                
                // Sanitize html if any
                var sanitizer = new HtmlSanitizer();
                var sanitizedDesc = sanitizer.Sanitize(bissue.Description);
                bissue.Description = sanitizedDesc;

                _context.Add(bissue);
                var project = await _context.Projects.FindAsync(bissue.ProjectId);
                project.ModifiedDate = DateTime.UtcNow;
                _context.Update(project);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", "Projects", new { id = bissue.ProjectId });
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", bissue.ProjectId);
            return View(bissue);
        }

        // GET: Bissues/Edit/5
        /// <summary>
        /// Edit takes a Bissue Id and returns a view with the specified Bissue
        /// </summary>
        /// <param name="id">Id of the Bissue to edit</param>
        /// <returns>ViewResult</returns>
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            /* Still need to verify user is owner
             * await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
             * */
            if (id == null)
            {
                return NotFound();
            }

            var bissue = await _context.Bissues.FindAsync(id);
            if (bissue == null)
            {
                return NotFound();
            }
            // Verify user is owner or admin
            if(User.FindFirst(ClaimTypes.NameIdentifier).Value != bissue.AppUserId && !User.IsInRole("Admin"))
            {
                return new ForbidResult();
            }
            
            return View(bissue);
        }

        // POST: Bissues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit POST takes a Bissue Id and a Bissue model and updates the record
        /// </summary>
        /// <param name="id">Bissue Id</param>
        /// <param name="bissue">Bissue model</param>
        /// <returns>Redirect to Index</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsOpen,AppUserId,AppUser,ProjectId,CreatedDate,ModifiedDate")] Bissue bissue)
        {
            if (id != bissue.Id)
            {
                return NotFound();
            }

            // Verify user is owner or admin
            var reqUser = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(reqUser.Id != bissue.AppUserId && !User.IsInRole("Admin"))
            {
                return new ForbidResult();
            }
            // If closed and null closed date, set closed date
            if(bissue.IsOpen == false && bissue.ClosedDate == null)
            {
                bissue.ClosedDate = DateTime.UtcNow;
            }

            // Sanitize html if any
            var sanitizer = new HtmlSanitizer();
            var sanitizedDesc = sanitizer.Sanitize(bissue.Description);
            bissue.Description = sanitizedDesc;

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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", bissue.ProjectId);
            return View(bissue);
        }

        // GET: Bissues/Delete/5
        /// <summary>
        /// Deletes a Bissue
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns></returns>
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

            // Verify user is owner or admin
            var reqUser = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(reqUser.Id != bissue.AppUserId && !User.IsInRole("Admin"))
            {
                return new ForbidResult();
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
            // Verify user is owner or admin
            var reqUser = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(reqUser.Id != bissue.AppUserId && !User.IsInRole("Admin"))
            {
                return new ForbidResult();
            }
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
