using job_applications.Data;
using job_applications.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace job_applications.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly MyDbContext _context;

        public JobController(MyDbContext context)
        {
            _context = context;
        }

        // Get Jobs by Company ID
        [HttpGet("by-company/{companyId}")]
        public async Task<IActionResult> GetJobsByCompanyId(int companyId)
        {
            var jobs = await _context.Jobs
                .Where(j => j.CompanyId == companyId)
                .ToListAsync();

            return Ok(jobs);
        }

        // Create a new Job
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] Job job)
        {
            // Check if the company exists
            var companyExists = await _context.Companies.AnyAsync(c => c.Id == job.CompanyId);

            if (!companyExists)
            {
                return NotFound("Company not found.");
            }

            // Add the new job
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJobsByCompanyId), new { companyId = job.CompanyId }, job);
        }

        // Update a Job
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] Job updatedJob)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound("Job not found.");
            }

            job.JobTitle = updatedJob.JobTitle ?? job.JobTitle;
            job.JobDescription = updatedJob.JobDescription ?? job.JobDescription;
            job.Requirements = updatedJob.Requirements ?? job.Requirements;
            job.PostedDate = updatedJob.PostedDate != default ? updatedJob.PostedDate : job.PostedDate;
            job.Salary = updatedJob.Salary != default ? updatedJob.Salary : job.Salary;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();

            return Ok(job);
        }


        // Delete a Job
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound("Job not found.");
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return Ok($"Job with ID {id} has been deleted.");
        }
    }
}
