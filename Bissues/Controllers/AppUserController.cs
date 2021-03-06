﻿using System;
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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bissues.Controllers
{
    [Authorize]
    /// <summary>
    /// AppUser controller handles AppUser requests, which returns a view 
    /// showing the user all of the Bissues and Messsages they have created, 
    /// along with any unread notifications they may have.
    /// </summary>
    public class AppUserController : Controller
    {
        private readonly ILogger<AppUserController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AppUserController(ILogger<AppUserController> logger, ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        /// <summary>
        /// Index returns a view with the users created content.
        /// </summary>
        /// <returns>ViewResult</returns>
        public async Task<IActionResult> Index()
        {
            var reqUser = await _userManager.FindByNameAsync(User.Identity.Name);
            // Shouldn't be null if they got past Authorize on the class
            if(reqUser == null)
            {
                return NotFound();
            }
            var model = GetAppUserAreaViewModel(reqUser.Id);
            return View(model);
        }
        /// <summary>
        /// Developer returns a view with a list of Bissues that have been 
        /// assigned to the developer.
        /// </summary>
        /// <returns>ViewResult</returns>
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> Developer()
        {
            var reqUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if(reqUser == null)
            {
                return NotFound();
            }
            var model = _context.Bissues.Where(b => b.AssignedDeveloperId == reqUser.Id).ToList();
            return View(model);
        }
        /// <summary>
        /// ReadNotification marks the notification as read and redirects to the 
        /// related Bissue's Details view.
        /// </summary>
        /// <param name="id">Notification Id</param>
        /// <returns>Redirect to related Bissue</returns>
        public async Task<IActionResult> ReadNotification(int id)
        {
            var notification = await _context.Notifications.Where(n => n.Id == id).FirstOrDefaultAsync();
            if(notification == null)
            {
                return RedirectToAction("Index");
            }
            var bissue = await _context.Bissues.Where(b => b.Id == notification.BissueId).FirstOrDefaultAsync();
            if(bissue == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _context.Remove(notification);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Bissues", new {id = bissue.Id});
        }
        /// <summary>
        /// Constructs and returns an AppUserAreaViewModel
        /// </summary>
        /// <param name="id">AppUserId</param>
        /// <returns>AppUserAreaViewModel</returns>
        private AppUserAreaViewModel GetAppUserAreaViewModel(string id)
        {
            return new AppUserAreaViewModel()
            {
                Bissues = _context.Bissues.Where(b => b.AppUserId == id).ToList(),
                Messages = _context.Messages.Where(m => m.AppUserId == id).ToList(),
                Notifications = _context.Notifications.Where(n => n.AppUserId == id).ToList()
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
