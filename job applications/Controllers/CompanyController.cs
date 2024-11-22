using job_applications.Data;
using job_applications.DTO;
using job_applications.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace job_applications.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration; // Add IConfiguration

        public CompanyController(MyDbContext context, IConfiguration configuration) // Inject IConfiguration
        {
            _context = context;
            _configuration = configuration; // Assign to the private variable
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CompanyDto companyDto)
        {
            if (companyDto == null || string.IsNullOrEmpty(companyDto.Name) || string.IsNullOrEmpty(companyDto.Email) || string.IsNullOrEmpty(companyDto.PasswordHash) || companyDto.Location == null)
            {
                return BadRequest("Invalid company data.");
            }

            // Map the DTO to the Company model
            var company = new Company
            {
                Name = companyDto.Name,
                Email = companyDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(companyDto.PasswordHash), // Hash the password
                Phone = companyDto.Phone,
                WebsiteURL = companyDto.WebsiteURL,
                Location = companyDto.Location
            };

            // Add the company to the context
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Invalid login data.");
            }

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Email == loginModel.Email);

            if (company == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, company.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Use injected IConfiguration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, company.Id.ToString()), // Assuming Id is the unique identifier
                    new Claim(ClaimTypes.Email, company.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration
                Issuer = _configuration["Jwt:Issuer"], // Use injected IConfiguration
                Audience = _configuration["Jwt:Audience"], // Use injected IConfiguration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            return Ok(company);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyById(int id, [FromBody] CompanyUpdateDto companyUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            // Only update the fields that are provided
            if (!string.IsNullOrEmpty(companyUpdateDto.Name))
            {
                company.Name = companyUpdateDto.Name;
            }

            if (!string.IsNullOrEmpty(companyUpdateDto.Email))
            {
                company.Email = companyUpdateDto.Email;
            }

            if (!string.IsNullOrEmpty(companyUpdateDto.Phone))
            {
                company.Phone = companyUpdateDto.Phone;
            }

            if (!string.IsNullOrEmpty(companyUpdateDto.WebsiteURL))
            {
                company.WebsiteURL = companyUpdateDto.WebsiteURL;
            }

            if (companyUpdateDto.Location != null)
            {
                company.Location = companyUpdateDto.Location;
            }

            if (!string.IsNullOrEmpty(companyUpdateDto.PasswordHash))
            {
                company.PasswordHash = companyUpdateDto.PasswordHash;
            }

            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            return Ok(company);
        }





    }
}
