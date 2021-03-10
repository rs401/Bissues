using System.Collections.Generic;
using Bissues.Models;

namespace Bissues.ViewModels
{
    public class AppUserAreaViewModel
    {
        public List<Bissue> Bissues { get; set; }
        public List<Message> Messages { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}