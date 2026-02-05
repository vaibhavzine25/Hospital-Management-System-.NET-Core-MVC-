using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using WebApplicationSampleTest2.Models;

namespace WebApplicationSampleTest2.Repository
{
    public class MedicineRepository : IMedicine
    {

        private readonly string _connectionString;

        public MedicineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
        }

        public void Delete(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_DeleteMedicine", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_MedicineId", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Medicine> GetAll()
        {
            var list = new List<Medicine>();
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_GetAllMedicines", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Medicine
                            {
                                MedicineId = Convert.ToInt32(reader["MedicineId"]),
                                MedicineName = reader["MedicineName"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Description = reader["Description"].ToString(),
                                Type = reader["Type"].ToString(),
                                Morning = Convert.ToBoolean(reader["Morning"]),
                                Afternoon = Convert.ToBoolean(reader["Afternoon"]),
                                Evening = Convert.ToBoolean(reader["Evening"]),
                            });
                        }
                    }
                }
            }
            return list;
        }

        public Medicine GetById(int id)
        {
            Medicine med = null;
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_GetMedicineById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_MedicineId", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            med = new Medicine
                            {
                                MedicineId = Convert.ToInt32(reader["MedicineId"]),
                                MedicineName = reader["MedicineName"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Description = reader["Description"].ToString(),
                                Type = reader["Type"].ToString(),
                                Morning = Convert.ToBoolean(reader["Morning"]),
                                Afternoon = Convert.ToBoolean(reader["Afternoon"]),
                                Evening = Convert.ToBoolean(reader["Evening"]),
                            };
                        }
                    }
                }
            }
            return med;
        }

        public void ADDMedicine(Medicine medicine)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_InsertMedicine", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_MedicineName", medicine.MedicineName);
                    cmd.Parameters.AddWithValue("p_Quantity", medicine.Quantity);
                    cmd.Parameters.AddWithValue("p_Description", medicine.Description);
                    cmd.Parameters.AddWithValue("p_Type", medicine.Type);
                    cmd.Parameters.AddWithValue("p_Morning", medicine.Morning);
                    cmd.Parameters.AddWithValue("p_Afternoon", medicine.Afternoon);
                    cmd.Parameters.AddWithValue("p_Evening", medicine.Evening);

                    cmd.ExecuteNonQuery();
                }
            }
        }

            public void Update(Medicine medicine)
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_UpdateMedicine", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_MedicineId", medicine.MedicineId);
                        cmd.Parameters.AddWithValue("p_MedicineName", medicine.MedicineName);
                        cmd.Parameters.AddWithValue("p_Quantity", medicine.Quantity);
                        cmd.Parameters.AddWithValue("p_Description", medicine.Description);
                        cmd.Parameters.AddWithValue("p_Type", medicine.Type);
                        cmd.Parameters.AddWithValue("p_Morning", medicine.Morning);
                        cmd.Parameters.AddWithValue("p_Afternoon", medicine.Afternoon);
                        cmd.Parameters.AddWithValue("p_Evening", medicine.Evening);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

        public int AddMedicine(Medicine model, int hospitalId, int subHospitalId)
        {
            int result = 0;

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_create_medicine", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_MedicineName", model.MedicineName);
                    cmd.Parameters.AddWithValue("@p_Quantity", model.Quantity);
                    cmd.Parameters.AddWithValue("@p_Description", model.Description);
                    cmd.Parameters.AddWithValue("@p_Type", model.Type);
                    cmd.Parameters.AddWithValue("@p_Morning", model.Morning);
                    cmd.Parameters.AddWithValue("@p_Afternoon", model.Afternoon);
                    cmd.Parameters.AddWithValue("@p_Evening", model.Evening);
                    cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
                    cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return result;
        }

        public List<Medicine> GetAllMedicine(int hospitalId, int subHospitalId)
        {
            List<Medicine> list = new List<Medicine>();

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_get_all_medicine", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
                    cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Medicine
                            {
                                MedicineId = Convert.ToInt32(dr["MedicineId"]),
                                MedicineName = dr["MedicineName"].ToString(),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                Description = dr["Description"].ToString(),
                                Type = dr["Type"].ToString(),
                                Morning = Convert.ToBoolean(dr["Morning"]),
                                Afternoon = Convert.ToBoolean(dr["Afternoon"]),
                                Evening = Convert.ToBoolean(dr["Evening"])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return list;
        }

        public Medicine GetMedicineById(int medicineId, int hospitalId, int subHospitalId)
        {
            Medicine medicine = null;

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_get_medicine_by_id", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_MedicineId", medicineId);
                    cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
                    cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            medicine = new Medicine
                            {
                                MedicineId = Convert.ToInt32(dr["MedicineId"]),
                                MedicineName = dr["MedicineName"].ToString(),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                Description = dr["Description"].ToString(),
                                Type = dr["Type"].ToString(),
                                Morning = Convert.ToBoolean(dr["Morning"]),
                                Afternoon = Convert.ToBoolean(dr["Afternoon"]),
                                Evening = Convert.ToBoolean(dr["Evening"])
                            };
                        }
                    }
                    con.Close();
                }
            }
            return medicine;
        }

        public int UpdateMedicine(Medicine model, int hospitalId, int subHospitalId)
        {
            int result = 0;

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_update_medicine", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_MedicineId", model.MedicineId);
                    cmd.Parameters.AddWithValue("@p_MedicineName", model.MedicineName);
                    cmd.Parameters.AddWithValue("@p_Quantity", model.Quantity);
                    cmd.Parameters.AddWithValue("@p_Description", model.Description);
                    cmd.Parameters.AddWithValue("@p_Type", model.Type);
                    cmd.Parameters.AddWithValue("@p_Morning", model.Morning);
                    cmd.Parameters.AddWithValue("@p_Afternoon", model.Afternoon);
                    cmd.Parameters.AddWithValue("@p_Evening", model.Evening);
                    cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
                    cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return result;
        }

        public int DeleteMedicine(int medicineId, int hospitalId, int subHospitalId)
        {
            int result = 0;

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_delete_medicine", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_MedicineId", medicineId);
                    cmd.Parameters.AddWithValue("@p_Hospital_Id", hospitalId);
                    cmd.Parameters.AddWithValue("@p_SubHospital_Id", subHospitalId);

                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return result;
        }
    }
}
