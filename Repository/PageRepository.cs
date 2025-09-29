using Microsoft.Data.SqlClient;
using Ext_Dynamic_Form.Models;
using System;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace Ext_Dynamic_Form.Repository
{
    public class PageRepository
    {
        private readonly string _connectionString;

        public PageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Page> GetAll(string action)
        {
            var list = new List<Page>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Page_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Page
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                Title = reader["Title"].ToString(),
                                AjaxURL = reader["AjaxURL"].ToString(),
                                Description = reader["Description"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public Page GetById(Int64 id,string action)
        {
            Page page = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Page_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            page = new Page
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                Title = reader["Title"].ToString(),
                                AjaxURL = reader["AjaxURL"].ToString(),
                                Description = reader["Description"].ToString()
                            };
                        }
                    }
                }
            }
            return page;
        }

        public Int64 Insert(Page page,string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Page_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", page.ID);
                    cmd.Parameters.AddWithValue("@Title", page.Title ?? "");
                    cmd.Parameters.AddWithValue("@AjaxURL", page.AjaxURL ?? "");
                    cmd.Parameters.AddWithValue("@Description", page.Description ?? "");
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    return Convert.ToInt64(cmd.ExecuteScalar());
                }
            }
        }


        public void Delete(Int64 id,string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Page_CRUD", con))
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
