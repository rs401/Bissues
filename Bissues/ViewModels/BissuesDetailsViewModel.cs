using System.Collections.Generic;
using Bissues.Models;

namespace Bissues.ViewModels
{
    public class BissuesDetailsViewModel
    {
        public Bissue Bissue { get; set; }
        public List<Message> Messages { get; set; }
        public int CurrentIndex { get; set; }
        public int PageCount { get; set; }
    }
}