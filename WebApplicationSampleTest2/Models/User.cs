using System;

namespace WebApplicationSampleTest2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public int HospitalId { get; set; }
        public int? SubHospitalId { get; set; } // Nullable for optional sub-hospital
        public string Type { get; set; }
        public string Role { get; set; }
        public bool isUpdate { get; set; }
        public string HospitalName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public int? MainHospitalId { get; set; }
      
    }
}
