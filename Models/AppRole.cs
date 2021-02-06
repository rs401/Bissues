using System;
using Microsoft.AspNetCore.Identity;

namespace Bissues.Models
{
    /// <summary>
    /// AppRole for internal use of creating roles
    /// </summary>
    public class AppRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}