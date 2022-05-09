using exorderproject_core.Helpers;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace exorderproject_api.DataAccess
{
    public class MainDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];

        internal List<CATEGORY> GetAllKategori(int id)
        {
            List<CATEGORY> kategoriler = new List<CATEGORY>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TCATEGORY where CATEGORY_C_ID=@CATEGORY_C_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@CATEGORY_C_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        kategoriler.Add(new CATEGORY()
                        {
                            CATEGORY_ID = reader["CATEGORY_ID"].milToInt32(),
                            CATEGORY_NAME = reader["CATEGORY_NAME"].milToString(),
                            CATEGORY_STATUS = reader["CATEGORY_STATUS"].milToInt32()
                            
                        });
                    }

                    con.Close();
                }
                return kategoriler;
            }
        }

        internal List<CATEGORY> GetAktifKategori(int id)
        {
            List<CATEGORY> kategoriler = new List<CATEGORY>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TCATEGORY where CATEGORY_STATUS=1 and CATEGORY_C_ID=@CATEGORY_C_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@CATEGORY_C_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        kategoriler.Add(new CATEGORY()
                        {
                            CATEGORY_ID = reader["CATEGORY_ID"].milToInt32(),
                            CATEGORY_NAME = reader["CATEGORY_NAME"].milToString()

                        });
                    }

                    con.Close();
                }
                return kategoriler;
            }
        }

        internal List<UNIT> GetAllUnit()
        {
            List<UNIT> birimler = new List<UNIT>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TUNIT ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        birimler.Add(new UNIT()
                        {
                            UNIT_ID = reader["UNIT_ID"].milToInt32(),
                            UNIT_NAME = reader["UNIT_NAME"].milToString(),
                            UNIT_DESCRIPTION = reader["UNIT_DESCRIPTION"].milToString()

                        });
                    }

                    con.Close();
                }
                return birimler;
            }
        }

        internal List<STATUS> GetAllStatus()
        {
            List<STATUS> durumlar = new List<STATUS>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TSTATUS ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();


                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        durumlar.Add(new STATUS()
                        {
                            STATUS_ID = reader["STATUS_ID"].milToInt32(),
                            STATUS_NAME = reader["STATUS_NAME"].milToString()

                        });
                    }

                    con.Close();
                }
                return durumlar;
            }
        }
    }
}