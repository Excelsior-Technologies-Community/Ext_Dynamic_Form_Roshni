using Microsoft.Data.SqlClient;
using System.Data;
using Ext_Dynamic_Form.Models;

namespace Ext_Dynamic_Form.Repository
{
    public class DynamicFormRepository
    {
        private readonly string _connectionString;

        public DynamicFormRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Int64 InsertOrUpdate(DynamicFormData model, string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_DynamicFormData_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@ActivityId", model.ActivityId);
                    cmd.Parameters.AddWithValue("@FieldName", model.FieldName ?? "");
                    cmd.Parameters.AddWithValue("@FieldValue", model.FieldValue ?? "");

                    con.Open();

                    if (action == "INSERT")
                    {
                        var newId = cmd.ExecuteScalar();
                        return Convert.ToInt32(newId);
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                        return model.ID;
                    }
                }
            }
        }

        public void Delete(Int64 id,string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_DynamicFormData_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@ID", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

       
        public List<DynamicFormData> GetAll(string action,Int64? activityId = null)
        {
            List<DynamicFormData> list = new List<DynamicFormData>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_DynamicFormData_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@ActivityId", (object)activityId ?? DBNull.Value);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DynamicFormData
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                ActivityId = reader["ActivityId"] != DBNull.Value ? Convert.ToInt32(reader["ActivityId"]) : 0,
                                FieldName = reader["FieldName"].ToString(),
                                FieldValue = reader["FieldValue"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
