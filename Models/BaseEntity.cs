using System;

namespace Bissues.Models
{
    /// <summary>
    /// BaseEntity is a base entity for Bissues, Projects, Messages and Notifications to have a created and modified date.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// CreatedDate holds the DateTime the entity was created.
        /// </summary>
        /// <value>the DateTime the entity was created</value>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// ModifiedDate holds the DateTime the entity was modified.
        /// </summary>
        /// <value>the DateTime the entity was modified</value>
        public DateTime ModifiedDate { get; set; }
        /* FK Owner of the BaseEntity */
        /// <summary>
        /// Owner of the BaseEntity
        /// </summary>
        /// <value>Owner of the BaseEntity</value>
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}