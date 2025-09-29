using Ext_Dynamic_Form.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ext_Dynamic_Form.Repository
{
    public class CityRepository
    {
        private readonly string _connectionString;

        public CityRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Int64 Insert(City city, string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_City_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", city.ID);
                    cmd.Parameters.AddWithValue("@CountryId", city.CountryId);
                    cmd.Parameters.AddWithValue("@StateId", city.StateId);
                    cmd.Parameters.AddWithValue("@CityName", city.CityName ?? "");
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    return Convert.ToInt64(cmd.ExecuteScalar());
                }
            }
        }

        public void Delete(Int64 id, string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_City_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<City> GetAll(string action)
        {
            var list = new List<City>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_City_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new City
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                CountryId = Convert.ToInt64(reader["CountryId"]),
                                StateId = Convert.ToInt64(reader["StateId"]),
                                CityName = reader["CityName"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public City GetById(long id,string action)
        {
            City city = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_City_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            city = new City
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                CountryId = Convert.ToInt64(reader["CountryId"]),
                                StateId = Convert.ToInt64(reader["StateId"]),
                                CityName = reader["CityName"].ToString()
                            };
                        }
                    }
                }
            }
            return city;
        }
    }
}
