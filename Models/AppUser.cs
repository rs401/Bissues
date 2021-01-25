using System;
using Microsoft.AspNetCore.Identity;

namespace Bissues.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
        public AppUser()
        {
            
        }
    }
}