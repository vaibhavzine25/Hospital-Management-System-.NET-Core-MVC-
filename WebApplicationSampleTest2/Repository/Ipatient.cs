using System.Collections.Generic;
using WebApplicationSampleTest2.Models;

namespace WebApplicationSampleTest2.Repository
{
    public interface Ipatient
    {
        int AddPatient(Patient model, int hospitalId, int subHospitalId);
        List<Patient> GetAllPatients(int hospitalId, int subHospitalId);
        Patient GetPatientById(int patientId, int hospitalId, int subHospitalId);
        int UpdatePatient(Patient model, int hospitalId, int subHospitalId);
        int DeletePatient(int patientId, int hospitalId, int subHospitalId);

        // Login method
        Patient Login(string email, string password);

        bool CheckEmail(string email);
        bool UpdatePassword(string email, string newPassword);

        

        bool SignupPatient(Patient patient, out string message);

    }
}
