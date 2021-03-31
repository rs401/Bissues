using System.Collections.Generic;
using Bissues.Models;

namespace Bissues.ViewModels
{
    public class AdminAreaViewModel
    {
        public List<Bissue> Bissues { get; set; }
        public List<Project> Projects { get; set; }
    }
}