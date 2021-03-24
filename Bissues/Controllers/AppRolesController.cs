using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bissues.Data;
using Bissues.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Bissues.Controllers
{
    [Authorize(Roles = "Admin")]
    /// <summary>
    /// AppRolesController to handle AppRoles requests
    /// </summary>
    public class AppRolesController : Controller
    {
        // private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        public AppRolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        // GET: AppRoles
        /// <summary>
        /// AppRoles Index
        /// </summary>
        /// <returns>Returns Index view with list of roles.</returns>
        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        // GET: AppRoles/Create
        /// <summary>
        /// Create AppRole's
        /// </summary>
        /// <returns>Returns the Create view</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Create AppRole POST
        /// </summary>
        /// <param name="appRole">The AppRole to be created.</param>
        /// <returns>Returns the create view</returns>
        public async Task<IActionResult> Create([Bind("Id,RoleName")] AppRole appRole)
        {
            var roleExist = await roleManager.RoleExistsAsync(appRole.RoleName);
            if(!roleExist)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(appRole.RoleName));
            }
            return View();
        }

        // GET: AppRoles/Edit/5
        /// <summary>
        /// AppRole Edit GET, returns the Edit view with a RoleEdit model populated with members and non-members of the specified role
        /// </summary>
        /// <param name="id">The AppRole to be edited</param>
        /// <returns>Returns the AppRoles Edit view with a created RoleEdit model</returns>
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            List<AppUser> allUsers = userManager.Users.ToList();
            foreach (AppUser user in allUsers)
            {
                if(user.UserName == "admin@admin.com")
                {
                    continue;
                }
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        // POST: AppRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// AppRole Edit POST, takes in a RoleModification model and adds or removes users from said role
        /// </summary>
        /// <param name="model">The RoleModification model</param>
        /// <returns>Success redirects to AppRoles Index, Failure returns to Role Edit view.</returns>
        public async Task<IActionResult> Edit(RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }
 
            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Edit(model.RoleId);
        }
    }
}
