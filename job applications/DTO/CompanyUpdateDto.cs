using job_applications.Models;

namespace job_applications.DTO
{
    public class CompanyUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WebsiteURL { get; set; }
        public Location Location { get; set; }
        public string PasswordHash { get; set; }
    }

}
