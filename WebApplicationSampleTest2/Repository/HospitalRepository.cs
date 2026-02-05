using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebApplicationSampleTest2.Models;

namespace WebApplicationSampleTest2.Repository
{
    public class HospitalRepository : IHospital
    {
        private readonly string _connectionString;

        public HospitalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
        }
        

        public List<Hospital> GetAllHospitals()
        {
            var list = new List<Hospital>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_GetAllHospital", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Hospital
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"]?.ToString(),
                                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                EmailId = reader["EmailId"]?.ToString(),
                                Logo = reader["Logo"]?.ToString(),
                                RegistrationNumber = reader["RegistrationNumber"]?.ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                IsSubHospital = Convert.ToBoolean(reader["IsSubHospital"]),
                                ParentHospitalId = reader["ParentHospitalId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["ParentHospitalId"]) : null,

                                // ✅ NEW: Parent Hospital Name
                                ParentHospitalName = reader["ParentHospitalName"] != DBNull.Value
                                                     ? reader["ParentHospitalName"].ToString()
                                                     : null
                            });
                        }
                    }
                }
            }

            return list;
        }

        // GET HOSPITAL BY ID
        public Hospital GetHospitalById(int id)
        {
                Hospital hospital = null;

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetHospitalById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_Id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hospital = new Hospital
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"]?.ToString(),
                                    PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                    EmailId = reader["EmailId"]?.ToString(),
                                    MetaLink = reader["MetaLink"]?.ToString(),
                                    InstaLink = reader["InstaLink"]?.ToString(),
                                    Logo = reader["Logo"]?.ToString(),
                                    RegistrationNumber = reader["RegistrationNumber"]?.ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    IsSubHospital = Convert.ToBoolean(reader["IsSubHospital"]),
                                    ParentHospitalId = reader["ParentHospitalId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["ParentHospitalId"]) : null
                               
                                };
                            }
                        }
                    }
                }

                return hospital;
        }

        // INSERT HOSPITAL
        public void InsertHospital(Hospital hospital)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_InsertHospital", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Name", hospital.Name);
                    cmd.Parameters.AddWithValue("@p_Description", hospital.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_PhoneNumber", hospital.PhoneNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_EmailId", hospital.EmailId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_MetaLink", hospital.MetaLink ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_InstaLink", hospital.InstaLink ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_Logo", hospital.Logo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_RegistrationNumber", hospital.RegistrationNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_IsActive", hospital.IsActive);
                    cmd.Parameters.AddWithValue("@p_IsSubHospital", hospital.IsSubHospital);
                    cmd.Parameters.AddWithValue("@p_ParentHospitalId", hospital.ParentHospitalId ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // UPDATE HOSPITAL
        public void UpdateHospital(Hospital hospital)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_UpdateHospital", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Id", hospital.Id);
                    cmd.Parameters.AddWithValue("@p_Name", hospital.Name);
                    cmd.Parameters.AddWithValue("@p_Description", hospital.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_PhoneNumber", hospital.PhoneNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_EmailId", hospital.EmailId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_MetaLink", hospital.MetaLink ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_InstaLink", hospital.InstaLink ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_Logo", hospital.Logo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_RegistrationNumber", hospital.RegistrationNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_IsActive", hospital.IsActive);
                    cmd.Parameters.AddWithValue("@p_IsSubHospital", hospital.IsSubHospital);
                    cmd.Parameters.AddWithValue("@p_ParentHospitalId", hospital.ParentHospitalId ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE HOSPITAL (Soft Delete)
        public void DeleteHospital(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_DeleteHospital", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Hospital> GetMainHospitals()
        {
            var list = new List<Hospital>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("sp_GetMainHospitals", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Hospital
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<Hospital> GetSubHospitalsByMainId(int mainHospitalId)
        {
            var list = new List<Hospital>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("GetSubHospitalsByMainId", conn)) // stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_MainHospitalId", mainHospitalId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Hospital
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name")
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}

