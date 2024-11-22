namespace job_applications.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobId { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public string Notes { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public User User { get; set; }
        public Job Job { get; set; }
    }
}
