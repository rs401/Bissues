using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
 
namespace Bissues.Models
{
    /// <summary>
    /// For internal use of editing roles, adding and removing users from a role
    /// </summary>
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
}