using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Bissues.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisaplayName { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}