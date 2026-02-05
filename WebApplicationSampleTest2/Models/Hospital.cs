using Microsoft.AspNetCore.Http;

namespace WebApplicationSampleTest2.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string MetaLink { get; set; }
        public string InstaLink { get; set; }
        public string Logo { get; set; }
        public string RegistrationNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsSubHospital { get; set; }
        public int? ParentHospitalId { get; set; }
        public string ParentHospitalName { get; set; }
        public bool isUpdate { get; set; }
        public IFormFile LogoFile { get; set; } 
    }
}
