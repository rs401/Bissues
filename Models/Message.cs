using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bissues.Models
{
    /// <summary>
    /// Message class holds the body of the message, the AppUser that created the message and the Bissue the message is related to.
    /// </summary>
    public class Message : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        /// <summary>
        /// Message body holds the text of the message
        /// </summary>
        /// <value>the text of the message</value>
        public string Body { get; set; }

        /* FK AppUser the message belongs to */
        /// <summary>
        /// The AppUser that created the message
        /// </summary>
        /// <value>The AppUser that created the message</value>
        public int AppUserId { get; set; }
        public virtual AppUser Owner { get; set; }

        /* FK Bissue the message belongs to */
        /// <summary>
        /// The Bissue the message is related to
        /// </summary>
        /// <value>The Bissue the message is related to</value>
        public int BissueId { get; set; }
        public virtual Bissue Bissue { get; set; }

        /////////////////////////////////////////////
        // Need to run ef migrations add for these models and then check the migration files and update
    }
}