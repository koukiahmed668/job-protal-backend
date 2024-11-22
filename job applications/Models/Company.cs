using System.ComponentModel.DataAnnotations;

namespace job_applications.Models
{

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WebsiteURL { get; set; }

        public Location Location { get; set; }

        public string PasswordHash { get; set; }

        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }

}
