using job_applications.Models;
using System.ComponentModel.DataAnnotations;

namespace job_applications.DTO
{
    public class CompanyDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]

        public string Phone { get; set; }
        public string WebsiteURL { get; set; }

        public Location Location { get; set; }
    }

}
