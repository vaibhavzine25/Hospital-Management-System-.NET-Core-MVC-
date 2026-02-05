using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using WebApplicationSampleTest2.Models;

namespace WebApplicationSampleTest2.Repository
{
    public class PatientService : Ipatient
    {
        private readonly string _connectionString;

        public PatientService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
        }

        public void CreatePatient(Patient patient)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_create_patient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("p_FirstName", patient.FirstName);
                    cmd.Parameters.AddWithValue("p_LastName", patient.LastName);
                    cmd.Parameters.AddWithValue("p_Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("p_Age", patient.Age);
                    cmd.Parameters.AddWithValue("p_PhoneNumber", patient.PhoneNumber);
                    cmd.Parameters.AddWithValue("p_Address", patient.Address);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePatient(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_delete_patient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Patient> GetAllPatients()
        {
            List<Patient> list = new List<Patient>();

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_getall_patient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Patient
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Gender = dr["Gender"].ToString(),
                                Age = dr["Age"].ToString(),
                                PhoneNumber = dr["PhoneNumber"].ToString(),
                                Address = dr["Address"].ToString(),
                                isUpdate = Convert.ToBoolean(dr["isUpdate"])
                            });
                        }
                    }
                }
            }

            return list;
        }

        public Patient GetPatientById(int id)
        {
            Patient patient = null;

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_get_patient_by_id", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Id", id);

                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            patient = new Patient
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Gender = dr["Gender"].ToString(),
                                Age = dr["Age"].ToString(),
                                PhoneNumber = dr["PhoneNumber"].ToString(),
                                Address = dr["Address"].ToString(),
                                isUpdate = Convert.ToBoolean(dr["isUpdate"])
                            };
                        }
                    }
                }
            }

            return patient;
        }

        public void UpdatePatient(Patient patient)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_update_patient_basic", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("p_Id", patient.Id);
                    cmd.Parameters.AddWithValue("p_FirstName", patient.FirstName);
                    cmd.Parameters.AddWithValue("p_LastName", patient.LastName);
                    cmd.Parameters.AddWithValue("p_Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("p_Age", patient.Age);
                    cmd.Parameters.AddWithValue("p_PhoneNumber", patient.PhoneNumber);
                    cmd.Parameters.AddWithValue("p_Address", patient.Address);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Patient Login(string email, string password)
        {
            Patient patient = null;

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_patient_login", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Stored procedure parameters
                    cmd.Parameters.AddWithValue("@p_email", email);
                    cmd.Parameters.AddWithValue("@p_password", password);

                    con.Open();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Matching record found → map to Patient object
                            patient = new Patient
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                // Add other fields if needed
                            };
                        }
                    }
                }
            }

            return patient; // null if login failed
        }

        public bool CheckEmail(string email)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_check_email", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_email", email);

                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        return dr.HasRows;
                    }
                }
            }
        }

        public bool UpdatePassword(string email, string newPassword)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_update_password", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_email", email);
                    cmd.Parameters.AddWithValue("@p_new_password", newPassword);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        

        public bool SignupPatient(Patient patient, out string message)
        {
            message = string.Empty;

            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("sp_patient_Signup", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@p_FirstName", patient.FirstName);
                        cmd.Parameters.AddWithValue("@p_LastName", patient.LastName);
                        cmd.Parameters.AddWithValue("@p_Gender", patient.Gender);
                        cmd.Parameters.AddWithValue("@p_Age", patient.Age);
                        cmd.Parameters.AddWithValue("@p_PhoneNumber", patient.PhoneNumber);
                        cmd.Parameters.AddWithValue("@p_Email", patient.Email);
                        cmd.Parameters.AddWithValue("@p_Password", patient.Password);
                        cmd.Parameters.AddWithValue("@p_Address", patient.Address);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        message = "Patient registered successfully";
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Email or Mobile duplicate error from SIGNAL
                message = ex.Message;
                return false;
            }
        }

        public int AddPatient(Patient model, int hospitalId, int subHospitalId)
        {
            using var con = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("sp_create_patient", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_FirstName", model.FirstName);
            cmd.Parameters.AddWithValue("@p_LastName", model.LastName);
            cmd.Parameters.AddWithValue("@p_Gender", model.Gender);
            cmd.Parameters.AddWithValue("@p_Age", model.Age);
            cmd.Parameters.AddWithValue("@p_PhoneNumber", model.PhoneNumber);
            cmd.Parameters.AddWithValue("@p_Address", model.Address);
            cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
            cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

            con.Open();
            return cmd.ExecuteNonQuery();
        }

        public List<Patient> GetAllPatients(int hospitalId, int subHospitalId)
        {

            List<Patient> list = new List<Patient>();
            using var con = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("sp_get_all_patient", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
            cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Patient
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    Gender = dr["Gender"].ToString(),
                    Age = dr["Age"].ToString(),
                    PhoneNumber = dr["PhoneNumber"].ToString(),
                    Address = dr["Address"].ToString()
                });
            }
            return list;
        }

        public Patient GetPatientById(int patientId, int hospitalId, int subHospitalId)
        {
            Patient patient = null;
            using var con = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("sp_get_patient_by_id", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_PatientId", patientId);
            cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
            cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                patient = new Patient
                {
                   Id = Convert.ToInt32(dr["Id"]),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    Gender = dr["Gender"].ToString(),
                    Age = dr["Age"].ToString(),
                    PhoneNumber = dr["PhoneNumber"].ToString(),
                    Address = dr["Address"].ToString()
                };
            }
            return patient;
        }

        public int UpdatePatient(Patient model, int hospitalId, int subHospitalId)
        {
            using var con = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("sp_update_patient", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_PatientId", model.Id);
            cmd.Parameters.AddWithValue("@p_FirstName", model.FirstName);
            cmd.Parameters.AddWithValue("@p_LastName", model.LastName);
            cmd.Parameters.AddWithValue("@p_Gender", model.Gender);
            cmd.Parameters.AddWithValue("@p_Age", model.Age);
            cmd.Parameters.AddWithValue("@p_PhoneNumber", model.PhoneNumber);
            cmd.Parameters.AddWithValue("@p_Address", model.Address);
            cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
            cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

            con.Open();
            return cmd.ExecuteNonQuery();
        }

        public int DeletePatient(int patientId, int hospitalId, int subHospitalId)
        {
            using var con = new MySqlConnection(_connectionString);
            using var cmd = new MySqlCommand("sp_delete_patient", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_PatientId", patientId);
            cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
            cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }

 }

