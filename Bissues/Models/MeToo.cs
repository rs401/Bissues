using System;

namespace Bissues.Models
{
    /// <summary>
    /// MeToo is a class model to hold a string ip address and a Bissue, that 
    /// represents someone at that ip address has also had similar experiences.
    /// </summary>
    public class MeToo
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public int BissueId { get; set; }
        public Bissue Bissue { get; set; }
    }
}