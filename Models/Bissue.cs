using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bissues.Models
{
    public class Bissue : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsOpen { get; set; } = true;
        public ICollection<Message> Messages { get; set; }

        /* FK Owner of the Bissue */
        public int AppUserId { get; set; }
        public virtual AppUser Owner { get; set; }

        /* FK Project the Bissue belongs to */
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

    }
}