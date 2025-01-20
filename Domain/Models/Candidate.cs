using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Candidate
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)] // Limit FirstName to 100 characters
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)] // Limit LastName to 100 characters
        public string LastName { get; set; }

        [Required]
        [MaxLength(320)] // Email length limited to 320 characters
        public string Email { get; set; }

        [MaxLength(20)] // Limit PhoneNumber to 20 characters
        public string PhoneNumber { get; set; }

        // Retain MAX length for free-form fields
        public string PreferredCallTime { get; set; }
        public string LinkedInProfile { get; set; }
        public string GitHubProfile { get; set; }
        public string FreeTextComment { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
