using System.Collections.Generic;
using Bissues.Models;

namespace Bissues.ViewModels
{
    public class AdminAreaViewModel
    {
        public List<Bissue> Bissues { get; set; }
        public List<AppUser> Users { get; set; }
        public List<AppRole> Roles { get; set; }
        public List<Project> Projects { get; set; }
        public List<Message> Messages { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}