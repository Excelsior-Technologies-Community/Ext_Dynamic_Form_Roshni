using Ext_Dynamic_Form.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace Ext_Dynamic_Form.Repository
{
    public class ActivityDetailRepository
    {
        private readonly string _connectionString;

        public ActivityDetailRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<ActivityDetail> GetAll(string action)
        {
            var list = new List<ActivityDetail>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActivitiesDetail_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ActivityDetail
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                ActivityDeatailId = Convert.ToInt64(reader["ActivityDeatailId"]),
                                ActivityId = Convert.ToInt64(reader["ActivityId"]),
                                Title = reader["Title"].ToString(),
                                ActionTypeId = Convert.ToInt64(reader["ActionTypeId"]),
                                PageMasterId = Convert.ToInt64(reader["PageMasterId"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public ActivityDetail GetById(long id,string action)
        {
            ActivityDetail detail = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActivitiesDetail_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            detail = new ActivityDetail
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                ActivityDeatailId = Convert.ToInt64(reader["ActivityDeatailId"]),
                                ActivityId = Convert.ToInt64(reader["ActivityId"]),
                                Title = reader["Title"].ToString(),
                                ActionTypeId = Convert.ToInt64(reader["ActionTypeId"]),
                                PageMasterId = Convert.ToInt64(reader["PageMasterId"])
                            };
                        }
                    }
                }
            }
            return detail;
        }

        public Int64 Insert(ActivityDetail detail, string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActivitiesDetail_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActivityDeatailId", detail.ActivityDeatailId);
                    cmd.Parameters.AddWithValue("@ActivityId", detail.ActivityId);
                    cmd.Parameters.AddWithValue("@Title", detail.Title ?? "");
                    cmd.Parameters.AddWithValue("@ActionTypeId", detail.ActionTypeId);
                    cmd.Parameters.AddWithValue("@PageMasterId", detail.PageMasterId);
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    return Convert.ToInt64(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(ActivityDetail detail,string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActivitiesDetail_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", detail.ID);
                    cmd.Parameters.AddWithValue("@ActivityDeatailId", detail.ActivityDeatailId);
                    cmd.Parameters.AddWithValue("@ActivityId", detail.ActivityId);
                    cmd.Parameters.AddWithValue("@Title", detail.Title ?? "");
                    cmd.Parameters.AddWithValue("@ActionTypeId", detail.ActionTypeId);
                    cmd.Parameters.AddWithValue("@PageMasterId", detail.PageMasterId);
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
                using (SqlCommand cmd = new SqlCommand("usp_ActivitiesDetail_CRUD", con))
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
