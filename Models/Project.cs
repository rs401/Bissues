using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bissues.Models
{
    public class Project : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        //Need collection of Bissues
        public virtual ICollection<Bissue> Bissues { get; set; }
        
        public Project()
        {
            
        }
    }
}