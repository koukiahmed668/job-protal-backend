namespace job_applications.Models
{
    public class Job
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Requirements { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
        public Location Location { get; set; } 
        public Category Category { get; set; } 
        public decimal Salary { get; set; } 
        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
