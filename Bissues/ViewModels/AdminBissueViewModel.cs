using System.Collections.Generic;
using Bissues.Models;

namespace Bissues.ViewModels
{
    public class AdminBissueViewModel
    {
        public Bissue Bissue { get; set; }
        public List<AppUser> Users { get; set; }
    }
}