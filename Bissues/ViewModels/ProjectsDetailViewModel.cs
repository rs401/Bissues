using System.Collections.Generic;
using Bissues.Models;

namespace Bissues.ViewModels
{
    public class ProjectsDetailViewModel
    {
        public Project Project { get; set; }
        public List<Bissue> Bissues { get; set; }
        public int CurrentIndex { get; set; }
        public int PageCount { get; set; }
    }
}