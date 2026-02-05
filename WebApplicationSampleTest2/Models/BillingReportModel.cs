using System;
using System.Collections.Generic;

namespace WebApplicationSampleTest2.Models
{
    public class BillingReportModel
    {
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public string PatientName { get; set; }
        public DateTime VisitTime { get; set; }
        public Dictionary<string, string> BillingDetails { get; set; } = new Dictionary<string, string>();
        public int TotalAmount { get; set; }
    }
}
