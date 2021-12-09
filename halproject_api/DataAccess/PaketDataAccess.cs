using halproject_core.DTO;
using halproject_core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace halproject_api.DataAccess
{
    public class PaketDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];

        internal List<int> GetForCache(int firma, int? paketDurum)
        {
            List<int> paketler = new List<int>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TFIRMAPAKET LEFT JOIN TPAKET ON TPAKET.PAKET_ID = TFIRMAPAKET.FIRMAPAKET_PAKET_ID WHERE FIRMAPAKET_FIRMA_ID = @FIRMAPAKET_FIRMA_ID ";
                sqlQuery += paketDurum.milToInt32() > 0 ? " AND PAKET_DURUM_ID = @PAKET_DURUM_ID " : "";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@FIRMAPAKET_FIRMA_ID", firma);
                    if (paketDurum.milToInt32() > 0) cmd.Parameters.AddWithValue("@PAKET_DURUM_ID", paketDurum.milToInt32());

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        paketler.Add(reader["FIRMAPAKET_PAKET_ID"].milToInt32());
                    }

                    con.Close();
                }
                return paketler;
            }
        }

        internal List<PAKET> GetAldigimPaketler(int firma)
        {
            List<PAKET> paketler = new List<PAKET>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TPAKET WHERE PAKET_ID IN (SELECT FIRMAPAKET_PAKET_ID FROM TFIRMAPAKET WHERE FIRMAPAKET_PAKETDURUM_ID = 2 AND FIRMAPAKET_FIRMA_ID = @FIRMAPAKET_FIRMA_ID)";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@FIRMAPAKET_FIRMA_ID", firma);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        paketler.Add(new PAKET()
                        {
                            ID = reader["PAKET_ID"].milToInt32(),
                            ADI = reader["PAKET_ADI"].milToString(),
                            IKON = reader["PAKET_IKON"].milToString(),
                            ACIKLAMA = reader["PAKET_ACIKLAMA"].milToString(),
                            DURUM_ID = reader["PAKET_DURUM_ID"].milToInt32(),
                            FIYAT = reader["PAKET_FIYAT"].milToDecimal()
                        });
                    }

                    con.Close();
                }
                return paketler;
            }
        }

        internal List<PAKET> GetAlabilecegimPaketler(int firma)
        {
            List<PAKET> paketler = new List<PAKET>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TPAKET WHERE PAKET_ID NOT IN (SELECT FIRMAPAKET_PAKET_ID FROM TFIRMAPAKET WHERE FIRMAPAKET_PAKETDURUM_ID = 2 AND FIRMAPAKET_FIRMA_ID = @FIRMAPAKET_FIRMA_ID)";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@FIRMAPAKET_FIRMA_ID", firma);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        paketler.Add(new PAKET()
                        {
                            ID = reader["PAKET_ID"].milToInt32(),
                            ADI = reader["PAKET_ADI"].milToString(),
                            IKON = reader["PAKET_IKON"].milToString(),
                            ACIKLAMA = reader["PAKET_ACIKLAMA"].milToString(),
                            DURUM_ID = reader["PAKET_DURUM_ID"].milToInt32(),
                            FIYAT = reader["PAKET_FIYAT"].milToDecimal()
                        });
                    }

                    con.Close();
                }
                return paketler;
            }
        }

        internal PAKET GetById(int id)
        {
            PAKET paket = new PAKET();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TPAKET WHERE PAKET_ID = @PAKET_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@PAKET_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        paket = new PAKET()
                        {
                            ID = reader["PAKET_ID"].milToInt32(),
                            ADI = reader["PAKET_ADI"].milToString(),
                            IKON = reader["PAKET_IKON"].milToString(),
                            ACIKLAMA = reader["PAKET_ACIKLAMA"].milToString(),
                            DURUM_ID = reader["PAKET_DURUM_ID"].milToInt32(),
                            FIYAT = reader["PAKET_FIYAT"].milToDecimal()
                        };
                    }

                    con.Close();
                }
                return paket;
            }
        }

        internal FIRMAPAKET Buy(FIRMAPAKET fp)
        {
            FIRMAPAKET insertedFp = new FIRMAPAKET();
            int id;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TFIRMAPAKET 
                    ( FIRMAPAKET_FIRMA_ID, FIRMAPAKET_PAKET_ID, FIRMAPAKET_PAKETDURUM_ID, FIRMAPAKET_CREATED_AT ) 
                    VALUES ( @FIRMAPAKET_FIRMA_ID, @FIRMAPAKET_PAKET_ID, @FIRMAPAKET_PAKETDURUM_ID, @FIRMAPAKET_CREATED_AT);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@FIRMAPAKET_FIRMA_ID", fp.FIRMA_ID);
                    cmd.Parameters.AddWithValue("@FIRMAPAKET_PAKET_ID", fp.PAKET_ID);
                    cmd.Parameters.AddWithValue("@FIRMAPAKET_PAKETDURUM_ID", fp.PAKETDURUM_ID);
                    cmd.Parameters.AddWithValue("@FIRMAPAKET_CREATED_AT", DateTime.Now);

                    id = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            insertedFp = GeFirmaPakettById(id);
            return insertedFp;
        }

        private FIRMAPAKET GeFirmaPakettById(int id)
        {
            FIRMAPAKET fp = new FIRMAPAKET();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM TFIRMAPAKET WHERE FIRMAPAKET_ID = @FIRMAPAKET_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@FIRMAPAKET_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        fp = new FIRMAPAKET()
                        {
                            ID = reader["FIRMAPAKET_ID"].milToInt32(),
                            FIRMA_ID = reader["FIRMAPAKET_FIRMA_ID"].milToInt32(),
                            PAKET_ID = reader["FIRMAPAKET_PAKET_ID"].milToInt32(),
                            PAKETDURUM_ID = reader["FIRMAPAKET_PAKETDURUM_ID"].milToInt32(),
                            CREATED_AT = reader["FIRMAPAKET_CREATED_AT"].milToDateTime()
                        };
                    }

                    con.Close();
                }
                return fp;
            }
        }
    }
}