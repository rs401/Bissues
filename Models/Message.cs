using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bissues.Models
{
    public class Message : BaseEntity
    {
        public int Id { get; set; }
        public string Body { get; set; }

        /* FK AppUser the message belongs to */
        public int AppUserId { get; set; }
        public virtual AppUser Owner { get; set; }

        /* FK Bissue the message belongs to */
        public int BissueId { get; set; }
        public virtual Bissue Bissue { get; set; }

        /////////////////////////////////////////////
        // Need to run ef migrations add for these models and then check the migration files and update
    }
}