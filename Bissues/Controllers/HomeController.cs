using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Bissues.Models;
using Bissues.ViewModels;
using Bissues.Data;

namespace Bissues.Controllers
{
    /// <summary>
    /// Home controller handles Home requests
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // var bissues = await _context.Bissues.Where(b => b.IsOpen == true).OrderByDescending(bm => bm.ModifiedDate).Take(5);
            var bissues = await _context.Bissues.Include(b => b.Project).OrderByDescending(b => b.IsOpen).ThenByDescending(b => b.ModifiedDate).Take(5).ToListAsync();
            var model = new IndexViewModel(){Bissues = bissues};
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
