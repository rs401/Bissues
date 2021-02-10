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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Bissues.Models.Project> Projects { get; set;}
        public DbSet<Bissues.Models.Bissue> Bissues { get; set; }
        public DbSet<Bissues.Models.Message> Messages { get; set; }
        public DbSet<Bissues.Models.AppRole> AppRole { get; set; }
        public DbSet<Bissues.Models.Notification> Notifications { get; set; }
        public DbSet<Bissues.Models.AppUser> AppUsers { get; set; }

        // public override int SaveChanges()
        // {
        //     var entries = ChangeTracker
        //         .Entries()
        //         .Where(e => e.Entity is BaseEntity && (
        //                 e.State == EntityState.Added
        //                 || e.State == EntityState.Modified));

        //     foreach (var entityEntry in entries)
        //     {
        //         ((BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;

        //         if (entityEntry.State == EntityState.Added)
        //         {
        //             ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
        //         }
        //     }
        //     // SetOwnerAndDates();
        //     return base.SaveChanges();
        // }

        // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        // {
        //     var entries = ChangeTracker
        //         .Entries()
        //         .Where(e => e.Entity is BaseEntity && (
        //                 e.State == EntityState.Added
        //                 || e.State == EntityState.Modified));

        //     foreach (var entityEntry in entries)
        //     {
        //         ((BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;

        //         if (entityEntry.State == EntityState.Added)
        //         {
        //             ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
        //         }
        //     }
        //     // SetOwnerAndDates();
        //     return await base.SaveChangesAsync();
        // }

        private void SetOwnerAndDates()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUsername = !string.IsNullOrEmpty(userId)
                ? userId
                : "Anonymous";
            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((BaseEntity)entityEntry.Entity).AppUserId = currentUsername;
                }
            }
        }
        
    }//END class ApplicationDbContext
}
