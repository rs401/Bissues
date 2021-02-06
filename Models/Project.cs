using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bissues.Models
{
    /// <summary>
    /// Project's will be applications that may have Bissues
    /// </summary>
    public class Project : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        /// <summary>
        /// The Project Name
        /// </summary>
        /// <value>The Project Name</value>
        public string Name { get; set; }
        [Required]
        /// <summary>
        /// The Project Description
        /// </summary>
        /// <value>The Project Description</value>
        public string Description { get; set; }

        //Need collection of Bissues
        /// <summary>
        /// Collection of Bissues related to the Project
        /// </summary>
        /// <value>Collection of Bissues related to the Project</value>
        public virtual ICollection<Bissue> Bissues { get; set; }
        
        public Project()
        {
            
        }
    }
}