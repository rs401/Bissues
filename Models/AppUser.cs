using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Bissues.Models
{
    /// <summary>
    /// AppUser class to extend IdentityUser with custom fields
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// The AppUser's First Name
        /// </summary>
        /// <value>The AppUser's First Name</value>
        public string FirstName { get; set; }
        /// <summary>
        /// The AppUser's Last Name
        /// </summary>
        /// <value>The AppUser's Last Name</value>
        public string LastName { get; set; }
        /// <summary>
        /// The AppUser's Display Name
        /// </summary>
        /// <value>The AppUser's Display Name</value>
        public string DisaplayName { get; set; }
        /// <summary>
        /// Collection of Bissues belonging to the user
        /// </summary>
        /// <value>Collection of Bissues belonging to the user</value>
        public ICollection<Bissue> Bissues { get; set; }
        /// <summary>
        /// Collection of Messages belonging to the user
        /// </summary>
        /// <value>Collection of Messages belonging to the user</value>
        public ICollection<Message> Messages { get; set; }
        /// <summary>
        /// Collection of Notifications belonging to the user
        /// </summary>
        /// <value>Collection of Notifications belonging to the user</value>
        public ICollection<Notification> Notifications { get; set; }
    }
}