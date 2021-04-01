using System;
using System.ComponentModel.DataAnnotations;

namespace Bissues.Models
{
    /// <summary>
    /// OfficialNote is a note about a specific commit directly related to the Bissue.
    /// </summary>
    public class OfficialNote : BaseEntity
    {
        public int Id { get; set; }
        public int BissueId { get; set; }
        public Bissue Bissue { get; set; }
        public string CommitId { get; set; }
        public string CommitURL { get; set; }
        [Required]
        public string Note { get; set; }
    }
}