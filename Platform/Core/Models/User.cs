using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Platform.Core.Models
{
    
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Key]
        public string Email { get; set; }
        [Required]
        public SecureString Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeactivatedAt { get; set; }

    }
}
