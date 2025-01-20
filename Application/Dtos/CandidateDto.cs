using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CandidateDto
    {
        [Required(ErrorMessage = "First name is required.")]
        public  string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
      
        public string PhoneNumber { get; set; }

        public string PreferredCallTime { get; set; }

        [Url]
        public string LinkedInProfile { get; set; }

        [Url]
        public string GitHubProfile { get; set; }

        [Required(ErrorMessage = "A comment is required.")]
        public string FreeTextComment { get; set; }
    }
}
