using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Bissues.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bissues.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Bissues.Models.Project> Projects { get; set;}
        public DbSet<Bissues.Models.Bissue> Bissues { get; set; }
        public DbSet<Bissues.Models.Message> Messages { get; set; }
        public DbSet<Bissues.Models.AppRole> AppRole { get; set; }
        public DbSet<Bissues.Models.Notification> Notifications { get; set; }
        public DbSet<Bissues.Models.AppUser> AppUsers { get; set; }
        public DbSet<Bissues.Models.MeToo> MeToos { get; set; }
        public DbSet<Bissues.Models.OfficialNote> OfficialNotes { get; set; }

    }//END class ApplicationDbContext
}
