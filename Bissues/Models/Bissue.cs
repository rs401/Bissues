using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bissues.Models
{
    /// <summary>
    /// Bissue a Bug/Issue
    /// </summary>
    public class Bissue : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        /// <summary>
        /// The bissue title
        /// </summary>
        /// <value>The bissue title</value>
        public string Title { get; set; }
        [Required]
        /// <summary>
        /// The bissue description
        /// </summary>
        /// <value>The bissue description</value>
        public string Description { get; set; }
        /// <summary>
        /// IsOpen holds if the Bissue is open or closed.
        /// </summary>
        /// <value>Bissue is open(true) or closed(false)</value>
        public bool IsOpen { get; set; } = true;
        /// <summary>
        /// Messages is a collection of messages related to the Bissue
        /// </summary>
        /// <value>collection of messages related to the Bissue</value>
        public ICollection<Message> Messages { get; set; }

        /* FK Project the Bissue belongs to */
        /// <summary>
        /// Project the Bissue belongs to
        /// </summary>
        /// <value>Project the Bissue belongs to</value>
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        /// <summary>
        /// Label if the Bissue is an "Issue" or a "Bug"
        /// </summary>
        /// <value>enum Issue=0,Bug=1</value>
        public BissueLabel Label { get; set; } = BissueLabel.Issue;
        /// <summary>
        /// If the Bissue is labeled as a Bug, there will be a developer 
        /// assigned to oversee a fix.
        /// </summary>
        /// <value>The AppUser.Id of the developer assigned to fix</value>
        public string AssignedDeveloperId { get; set; } = "";
        /// <summary>
        /// The datetime the Bissue was marked closed
        /// </summary>
        /// <value>The datetime the Bissue was marked closed</value>
        public DateTime? ClosedDate { get; set; }

    }
}