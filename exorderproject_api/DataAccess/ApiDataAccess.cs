using exorderproject_core.DTO;
using exorderproject_core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace exorderproject_api.DataAccess
{
    public class ApiDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];

        internal APIKULLANICILOG GetApiLoginByToken(string token)
        {
            APIKULLANICILOG log = new APIKULLANICILOG();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TAPIKULLANICILOG WHERE APIKULLANICILOG_TOKEN = @APIKULLANICILOG_TOKEN";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@APIKULLANICILOG_TOKEN", token);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        log = new APIKULLANICILOG()
                        {
                            ID = reader["APIKULLANICILOG_ID"].milToInt32(),
                            TOKEN = reader["APIKULLANICILOG_TOKEN"].milToString(),
                            TARIH = reader["APIKULLANICILOG_TARIH"].milToDateTime(),
                            SONTARIH = reader["APIKULLANICILOG_SONTARIH"].milToDateTime()
                        };
                    }
                    else
                        log = null;

                    con.Close();
                }
                return log;
            }
        }

        internal APIKULLANICI CheckApiUser(string username, string password)
        {
            APIKULLANICI apiKullanici = new APIKULLANICI();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT TOP 1 ( CASE WHEN TAPIKULLANICILOG.APIKULLANICILOG_SONTARIH > GETDATE() AND APIKULLANICILOG_SONTARIH IS NOT NULL THEN 0 ELSE 1 END ) AS CONTINUE_TIME, 
                TAPIKULLANICI.APIKULLANICI_ID, TAPIKULLANICILOG.APIKULLANICILOG_TOKEN 
                FROM TAPIKULLANICI 
                LEFT JOIN TAPIKULLANICILOG ON TAPIKULLANICI.APIKULLANICI_ID = TAPIKULLANICILOG.APIKULLANICI_ID 
                WHERE APIKULLANICI_KULLANICI_ADI = @APIKULLANICI_KULLANICI_ADI AND APIKULLANICI_SIFRE = @APIKULLANICI_SIFRE AND APIKULLANICI_DURUM_ID = 1 
                ORDER BY APIKULLANICILOG_ID DESC";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@APIKULLANICI_KULLANICI_ADI", username);
                    cmd.Parameters.AddWithValue("@APIKULLANICI_SIFRE", password);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        apiKullanici.ID = reader["APIKULLANICI_ID"].milToInt32();
                        apiKullanici.CONTINUE_TIME = reader["CONTINUE_TIME"].milToInt32();
                        apiKullanici.TOKEN = reader["APIKULLANICILOG_TOKEN"].milToString();
                    }
                    else
                        apiKullanici = null;

                    con.Close();
                }
                return apiKullanici;
            }
        }

        internal int CreateLog(APIKULLANICILOG log)
        {
            int logId = 0;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TAPIKULLANICILOG
                    ( APIKULLANICI_ID, APIKULLANICILOG_TARIH, APIKULLANICILOG_TOKEN, APIKULLANICILOG_SONTARIH )
                    VALUES ( @APIKULLANICI_ID, @APIKULLANICILOG_TARIH, @APIKULLANICILOG_TOKEN, @APIKULLANICILOG_SONTARIH );  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@APIKULLANICI_ID", log.KULLANICI_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@APIKULLANICILOG_TARIH", log.TARIH.milToDateTime());
                    cmd.Parameters.AddWithValue("@APIKULLANICILOG_TOKEN", log.TOKEN.milToString());
                    cmd.Parameters.AddWithValue("@APIKULLANICILOG_SONTARIH", log.SONTARIH.milToDateTime());

                    logId = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            return logId;
        }
    }
}