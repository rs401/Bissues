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
        //https://www.youtube.com/watch?v=Pi46L7UYP8I&t=1079s
        // [Key]
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

        /* FK Owner of the Bissue */
        // /// <summary>
        // /// Owner of the Bissue
        // /// </summary>
        // /// <value>Owner of the Bissue</value>
        // public string AppUserId { get; set; }
        // public AppUser Owner { get; set; }

        /* FK Project the Bissue belongs to */
        /// <summary>
        /// Project the Bissue belongs to
        /// </summary>
        /// <value>Project the Bissue belongs to</value>
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

    }
}