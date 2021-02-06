using System.ComponentModel.DataAnnotations;
 
namespace Bissues.Models
{
    /// <summary>
    /// For internal use of editing roles, adding and removing users from a role
    /// </summary>
    public class RoleModification
    {
        [Required]
        public string RoleName { get; set; }
 
        public string RoleId { get; set; }
 
        public string[] AddIds { get; set; }
 
        public string[] DeleteIds { get; set; }
    }
}