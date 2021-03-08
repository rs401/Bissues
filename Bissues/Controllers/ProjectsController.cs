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
using System.Dynamic;
using Bissues.ViewModels;

namespace Bissues.Controllers
{
    /// <summary>
    /// ProjectsController handles Projects requests
    /// </summary>
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        /// <summary>
        /// Project Index GET View displays a list of all Projects
        /// </summary>
        /// <returns>Index view with a list of all Projects</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.Include("Bissues").ToListAsync());
        }

        // GET: Projects/Details/5
        /// <summary>
        /// Project Details GET view displays Project details
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns>Details view with project details</returns>
        public async Task<IActionResult> Details(int? id, int? currentIndex)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            if(currentIndex == null)
            {
                return View(GetDetailViewModel((int)id,1));
            }
            return View(GetDetailViewModel((int)id,(int)currentIndex));
        }

        private ProjectsDetailViewModel GetDetailViewModel(int id, int index)
        {
            int maxRows = 10;
            ProjectsDetailViewModel pdvModel = new ProjectsDetailViewModel();
            var project = _context.Projects.Find(id);
            pdvModel.Project = project;
            var bissues = _context.Bissues.Where(b => b.ProjectId == id).OrderByDescending(b => b.IsOpen).ThenByDescending(b => b.ModifiedDate).ToList();
            pdvModel.IssueCount = bissues.Where(b => b.Label == BissueLabel.Issue).Count();
            pdvModel.BugCount = bissues.Where(b => b.Label == BissueLabel.Bug).Count();
            pdvModel.BugOpened = bissues.Where(b => b.Label == BissueLabel.Bug && b.IsOpen == true).Count();
            pdvModel.BugClosed = bissues.Where(b => b.Label == BissueLabel.Bug && b.IsOpen == false).Count();
            pdvModel.IssueOpened = bissues.Where(b => b.Label == BissueLabel.Issue && b.IsOpen == true).Count();
            pdvModel.IssueClosed = bissues.Where(b => b.Label == BissueLabel.Issue && b.IsOpen == false).Count();
            pdvModel.Bissues = bissues.Skip((index - 1) * maxRows).Take(maxRows).ToList();
            pdvModel.CurrentIndex = index;
            pdvModel.PageCount = (bissues.Count / maxRows) + 1;
            return pdvModel;
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Project Create GET view displays a form to create a Project, Only "Admin" role can create projects
        /// </summary>
        /// <returns>Project Create view</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Project Create POST view takes a Project, sets created/modified dates and saves Project to db.
        /// </summary>
        /// <param name="project">The Project</param>
        /// <returns>Create view</returns>
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreatedDate,ModifiedDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.CreatedDate = DateTime.UtcNow;
                project.ModifiedDate = DateTime.UtcNow;
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Project Edit GET view displays form to edit Project
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns>Project Edit GET view</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CreatedDate"] = project.CreatedDate;
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Project Edit POST view takes the project and sets the ModifiedDate and updates the db
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <param name="project">Project model with edited information</param>
        /// <returns>Edit view</returns>
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedDate,ModifiedDate")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    project.ModifiedDate = DateTime.UtcNow;
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Project Delete GET view
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns>Delete view</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Project DeleteConfirmed POST view 
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns>Project Index view</returns>
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
