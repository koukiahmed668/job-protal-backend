using job_applications.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; // This is the missing directive

namespace job_applications.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>()
               .HasOne<Company>() // Specify the related entity
               .WithMany(c => c.Jobs)
               .HasForeignKey(j => j.CompanyId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
