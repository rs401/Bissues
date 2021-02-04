using System;

namespace Bissues.Models
{
    public class Notification : BaseEntity
    {
        public int Id { get; set; }
        public bool IsUnread { get; set; } = true;

        //FK User and Bissue
        /* FK AppUser the Notification belongs to */
        public int AppUserId { get; set; }
        public virtual AppUser Owner { get; set; }
        /* FK Bissue the Notification belongs to */
        public int BissueId { get; set; }
        public virtual Bissue Bissue { get; set; }
    }
}