using AutoMapper;
using job_applications.DTO;

namespace job_applications.Models
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyDto, Company>();
            CreateMap<CompanyUpdateDto, Company>();
        }


    }
}
