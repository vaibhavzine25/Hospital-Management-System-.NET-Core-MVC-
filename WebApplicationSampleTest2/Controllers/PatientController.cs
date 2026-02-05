using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationSampleTest2.Models;
using WebApplicationSampleTest2.Repository;

namespace WebApplicationSampleTest2.Controllers
{
    public class PatientController : Controller
    {
        public static List<Patient> lstPatient = new List<Patient>();
        public static List<string> _PreAuthSymTomsList = new List<string> { };
        public static List<tablet> _MedicinesList = new List<tablet> { };
        public static Dictionary<string, string> _BillingDetails = new Dictionary<string, string>();
        private readonly Ipatient _patientRepo;

        public PatientController(Ipatient patientRepo)
        {
            _patientRepo = patientRepo;
        }

        public IActionResult PrintPriscription(string Id)
        {
            try
            {
                Patient patient = lstPatient.Where(w => w.FirstName.ToLower() == Id.ToLower()).Select(s => s).FirstOrDefault();
                var dsdsw = patient;

                if (patient.symptoms != null)
                    _PreAuthSymTomsList = patient.symptoms;

                if (patient.Medicineslist != null)
                    _MedicinesList = patient.Medicineslist;

                if (patient.BillingDetails != null)
                    _BillingDetails = patient.BillingDetails;

                PatientReportModel _model = new PatientReportModel();

                _model.VisitTIme = DateTime.Now;
                _model.PatientName = patient.FirstName + " " + patient.LastName;
                _model.Age = patient.Age;
                _model.BP = patient.BP;
                _model.Pluse = patient.pulse;
                _model.Diagnosis = patient.Investigation;
                _model.tablate = patient.Medicineslist;
                _model.ReportDetail = patient.ReportDetail;

                return View(_model);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index");
        }
        public IActionResult Print([Bind] PatientReportModel _Patient)
        {
            string FirstName = _Patient.PatientName.Split(' ')[0];
            if (_Patient.tablate.Count == 0)
                _Patient.tablate = lstPatient.Where(w => w.FirstName == FirstName).Select(s => s.Medicineslist).FirstOrDefault();

            PriscriptionReport priscriptionReport = new PriscriptionReport();
            priscriptionReport.GenrateReport(_Patient);

            return RedirectToAction("Index");

        }
        public IActionResult BillingofPatient(string Id)
        {
            Patient patient = lstPatient.Where(w => w.FirstName.ToLower() == Id.ToLower()).Select(s => s).FirstOrDefault();
            var dsdsw = patient;

            if (patient.symptoms != null)
                _PreAuthSymTomsList = patient.symptoms;

            if (patient.Medicineslist != null)
                _MedicinesList = patient.Medicineslist;

            if (patient.BillingDetails != null)
                _BillingDetails = patient.BillingDetails;

            return View(patient);
        }

        [HttpGet]
        public IActionResult EditAsOPD(string id)
        {
            // 1️⃣ Safety check
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Patient identifier missing");


            int hospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("HospitalId"));
            int subHospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("SubHospitalId"));

            // 2️⃣ Make sure patient list is loaded
            if (lstPatient == null || !lstPatient.Any())
            {
                lstPatient = _patientRepo.GetAllPatients( hospitalId, subHospitalId);
            }

            // 3️⃣ Find patient (case-insensitive)
            var patient = lstPatient.FirstOrDefault(p =>
                p.FirstName != null &&
                p.FirstName.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
                return NotFound("Patient not found");

            // 4️⃣ Static master data (OPD symptoms)
            ViewBag.SymptomsMain = new List<string>
    {
        "abdomen",
        "chest",
        "skin",
        "head"
    };

            // 5️⃣ Previously selected data
            ViewBag.PreAuthSymptoms = patient.symptoms ?? new List<string>();
            _PreAuthSymTomsList = ViewBag.PreAuthSymptoms;

            // 6️⃣ Send patient to view
            return View(patient);
        }





        public IActionResult EditAsIPD(string Id)
        {
            var sdsdsd = Id;
            Patient patient = lstPatient.Where(w => w.FirstName.ToLower() == Id.ToLower()).Select(s => s).FirstOrDefault();
            var dsdsw = patient;
            return RedirectToAction("Index");
        }
        public IActionResult Index(string search = "", int page = 1)
        {
            int pageSize = 5;

            int hospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("HospitalId"));
            int subHospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("SubHospitalId"));

            // 1️⃣ Get patients hospital-wise
            List<Patient> patients =
                _patientRepo.GetAllPatients(hospitalId, subHospitalId);

            // 2️⃣ Search
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                patients = patients.Where(p =>
                    (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.LastName) && p.LastName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.Gender) && p.Gender.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.PhoneNumber) && p.PhoneNumber.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.Address) && p.Address.ToLower().Contains(search))
                ).ToList();
            }

            // 3️⃣ Pagination
            int totalRecords = patients.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            List<Patient> pagedPatients = patients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 4️⃣ ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;

            return View(pagedPatients);
        }

        public IActionResult Create()
        {
            Patient patient = new Patient();
            return View(patient);
        }

        public IActionResult SavePatientDetails([Bind] Patient _Patient)
        {

            lstPatient.Add(_Patient);

            return RedirectToAction("Index", "Patient");
        }

        public IActionResult UpdateBilling([Bind] Patient _Patient)
        {
            _Patient.BillingDetails = _BillingDetails;
            _Patient.Status = "Billing Completed";
            var _newpatianet = lstPatient.Where(w => w.FirstName.ToLower() == _Patient.FirstName.ToLower()).Select(s => s).FirstOrDefault();
            lstPatient.Remove(_newpatianet);
            lstPatient.Add(_Patient);

            return RedirectToAction("Index", "Patient");
        }
        public IActionResult UpdatePatientDetails([Bind] Patient _Patient)
        {
            if (string.IsNullOrEmpty(_Patient.ReportDetail))
            {
                _Patient.Status = "Visiting Progress";
            }
            else
                _Patient.Status = "Completed";

            _Patient.symptoms = _PreAuthSymTomsList;
            _Patient.symptomsmain = _PreAuthSymTomsList;
            _Patient.Medicineslist = _MedicinesList;
            var _newpatianet = lstPatient.Where(w => w.FirstName.ToLower() == _Patient.FirstName.ToLower()).Select(s => s).FirstOrDefault();
            lstPatient.Remove(_newpatianet);
            lstPatient.Add(_Patient);

            return RedirectToAction("Index", "Patient");
        }

        public IActionResult SaveSymtons(string Command)
        {
            if (!string.IsNullOrEmpty(Command))
            {
                _PreAuthSymTomsList.Add(Command);
            }

            return PartialView("_PreAuth", _PreAuthSymTomsList);
        }

        public IActionResult DeleteSymtons(string Command)
        {
            if (!string.IsNullOrEmpty(Command))
            {
                _PreAuthSymTomsList.Remove(Command);
            }

            return PartialView("_PreAuth", _PreAuthSymTomsList);
        }


        public IActionResult SaveMedicines(string Command, string _Morning, string _Afternoon, string _Evening)
        {
            if (!string.IsNullOrEmpty(Command))
            {
                tablet objtablet = new tablet();
                objtablet.TablateName = Command;
                objtablet.Morning = _Morning;
                objtablet.Afternoon = _Afternoon;
                objtablet.Evening = _Evening;

                _MedicinesList.Add(objtablet);
            }

            return PartialView("_Medicine", _MedicinesList);
        }

        public IActionResult DeleteMedicines(string Command)
        {
            if (!string.IsNullOrEmpty(Command))
            {
                tablet objtablet = _MedicinesList.Select(s => s).Where(w => w.TablateName == Command).FirstOrDefault();
                _MedicinesList.Remove(objtablet);
            }

            return PartialView("_Medicine", _MedicinesList);
        }

        public IActionResult SaveBillingDetails(string Command)
        {
            int Total = 0;
            if (!string.IsNullOrEmpty(Command))
            {
                if (Command.ToLower().Contains("opd"))
                    _BillingDetails.Add(Command, "500");

                if (Command.ToLower().Contains("nursing"))
                    _BillingDetails.Add(Command, "200");

                if (Command.ToLower().Contains("dressing"))
                    _BillingDetails.Add(Command, "100");

                if (Command.ToLower().Contains("other"))
                    _BillingDetails.Add(Command, "300");
            }
            foreach (KeyValuePair<string, string> kvp in _BillingDetails)
            {
                Total = Total + Convert.ToInt32(kvp.Value);
            }
            ViewData["TotalBill"] = Total;
            return PartialView("_TotalBill", _BillingDetails);
        }

        public IActionResult DeleteBillingDetails(string Command)
        {
            int Total = 0;

            if (!string.IsNullOrEmpty(Command))
            {
                _BillingDetails.Remove(Command);
            }
            foreach (KeyValuePair<string, string> kvp in _BillingDetails)
            {
                Total = Total + Convert.ToInt32(kvp.Value);
            }
            ViewData["TotalBill"] = Total;
            return PartialView("_TotalBill", _BillingDetails);
        }



        [HttpPost]
        public IActionResult CreatePatientDetails(Patient model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model); // same form
            }

            int hospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("HospitalId"));
            int subHospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("SubHospitalId"));

            if (model.Id == 0)
            {
                // CREATE
                _patientRepo.AddPatient(model, hospitalId, subHospitalId);
                TempData["SuccessMessage"] = "Patient created successfully!";
            }
            else
            {
                // UPDATE
                _patientRepo.UpdatePatient(model, hospitalId, subHospitalId);
                TempData["SuccessMessage"] = "Patient updated successfully!";
            }

            return RedirectToAction("PatientList");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                int hospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("HospitalId"));
                int subHospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("SubHospitalId"));

                _patientRepo.DeletePatient(id, hospitalId, subHospitalId);
                TempData["SuccessMessage"] = "Patient deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("PatientList");
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            int hospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("HospitalId"));
            int subHospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("SubHospitalId"));

            Patient patient = _patientRepo.GetPatientById(id, hospitalId, subHospitalId);

            if (patient == null)
            {
                return NotFound();
            }

            patient.isUpdate = true; // 🔑 update mode
            return View("Create", patient); // same Create.cshtml
        }

        public IActionResult PatientList(string search = "", int page = 1)
        {
            int pageSize = 5;

            int hospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("HospitalId"));
            int subHospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("SubHospitalId"));

            // 1️⃣ Get all patients for current hospital
            List<Patient> patients = _patientRepo
                                        .GetAllPatients(hospitalId, subHospitalId);

            // 2️⃣ SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                patients = patients.Where(p =>
                    (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.LastName) && p.LastName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.Gender) && p.Gender.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.PhoneNumber) && p.PhoneNumber.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(p.Address) && p.Address.ToLower().Contains(search))
                ).ToList();
            }

            // 3️⃣ PAGINATION
            int totalRecords = patients.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            List<Patient> pagedPatients = patients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 4️⃣ ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;

            return View(pagedPatients);
        }

    }
}
