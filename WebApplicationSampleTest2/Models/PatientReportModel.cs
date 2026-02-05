using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSampleTest2.Models
{
    public class PatientReportModel
    {
        public string Hostpital_Name { get; set; }
        public string Address { get; set; }
        public string DrName { get; set; }
        public string DrEducation { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string BP { get; set; }
        public string Pluse { get; set; }
        public string Diagnosis { get; set; }
        public DateTime VisitTIme { get; set; }

        public List<tablet> tablate { get; set; }

        public string ReportDetail { get; set; }
    }

    public class tablet
    {
        public string TablateName { get; set; }
        public string Morning { get; set; }
        public string Afternoon { get; set; }
        public string Evening { get; set; }
    }
}
