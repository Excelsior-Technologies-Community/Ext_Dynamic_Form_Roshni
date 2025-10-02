using Ext_Dynamic_Form.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace Ext_Dynamic_Form.Repository
{
    public class StateRepository
    {
        private readonly string _connectionString;

        public StateRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<State> GetAll(string action)
        {
            var list = new List<State>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_State_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new State
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                CountryId = Convert.ToInt64(reader["CountryId"]),
                                StateName = reader["StateName"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public State GetById(Int64 id, string action)
        {
            State state = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_State_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            state = new State
                            {
                                ID = Convert.ToInt64(reader["ID"]),
                                CountryId = Convert.ToInt64(reader["CountryId"]),
                                StateName = reader["StateName"].ToString()
                            };
                        }
                    }
                }
            }
            return state;
        }

        public Int64 Insert(State state, string action)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_State_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", state.ID);
                    cmd.Parameters.AddWithValue("@CountryId", state.CountryId);
                    cmd.Parameters.AddWithValue("@StateName", state.StateName ?? "");
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
                using (SqlCommand cmd = new SqlCommand("usp_State_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Action", action);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<State> GetCountryByState(string action,Int64? countryId = null)
        {
            var list = new List<State>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_State_CRUD", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@CountryId", countryId.HasValue ? (object)countryId.Value : DBNull.Value);
                    

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new State
                            {
                                ID = (Int64)rdr["ID"],
                                StateName = rdr["StateName"].ToString(),
                                CountryId = (Int64)rdr["CountryId"]
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
