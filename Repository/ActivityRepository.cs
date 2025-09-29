using Microsoft.Data.SqlClient;
using System;
using System.Data;
using Ext_Dynamic_Form.Models;
using static System.Collections.Specialized.BitVector32;

namespace Ext_Dynamic_Form.Repository
{
    public class ActivityRepository
    {
        private readonly string _connectionString;

        public ActivityRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Activity> GetAll(string action)
        {
            var list = new List<Activity>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Activities_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Activity
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Title = reader["Title"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public Activity GetById(Int64 id,string action)
        {
            Activity activity = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Activities_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            activity = new Activity
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Title = reader["Title"].ToString()
                            };
                        }
                    }
                }
            }
            return activity;
        }

        public Int64 Insert(Activity activity, string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Activities_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", activity.ID);
                    cmd.Parameters.AddWithValue("@Title", activity.Title ?? "");
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Activity activity,string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Activities_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", activity.ID);
                    cmd.Parameters.AddWithValue("@Title", activity.Title ?? "");
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Int64 id,string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Activities_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
