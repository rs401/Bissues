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
        public int BugCount { get; set; }
        public int IssueCount { get; set; }
        public int BugOpened { get; set; }
        public int BugClosed { get; set; }
        public int IssueOpened { get; set; }
        public int IssueClosed { get; set; }
    }
}