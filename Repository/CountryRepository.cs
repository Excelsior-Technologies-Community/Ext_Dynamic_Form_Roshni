using Ext_Dynamic_Form.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Ext_Dynamic_Form.Repository
{
    public class CountryRepository
    {
        private readonly string _connectionString;

        public CountryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Int64 Insert(Country country,string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Country_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", country.ID);
                    cmd.Parameters.AddWithValue("@CountryName", country.CountryName ?? "");
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    return Convert.ToInt64(cmd.ExecuteScalar());
                }
            }
        }

        public void Delete(Int64 id,string action)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Country_CRUD", conn))
                {
                    cmd.CommandType= CommandType.StoredProcedure;   
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Country> GetAll(string action)
        {
            var countries = new List<Country>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Country_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            countries.Add(new Country
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                CountryName = reader["CountryName"].ToString()
                            });
                        }
                    }
                }
            }
            return countries;
        }

        public Country GetById(Int64 id,string action) { 
            Country country = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Country_CRUD", conn))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read())
                        {
                            country = new Country
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                CountryName = reader["CountryName"].ToString()

                            };
                        }
                    }
                }
            }
            return country;
        }
    }
}
