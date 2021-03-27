using System;

namespace Bissues.Models
{
    public class MeToo
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public int BissueId { get; set; }
        public Bissue Bissue { get; set; }
    }
}