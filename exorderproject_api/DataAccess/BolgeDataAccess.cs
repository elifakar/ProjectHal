using exorderproject_api.Helpers;
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
    public class BolgeDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        EncryptionAndDecryptionHelper EncryptionAndDecryption = new EncryptionAndDecryptionHelper();

        internal List<BOLGE> GetAll(int id)
        {
            List<BOLGE> bolgeBilgileri = new List<BOLGE>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM  dbo.TREGION  INNER JOIN  dbo.TSTATUS ON dbo.TREGION.REGION_STATUS = dbo.TSTATUS.STATUS_ID WHERE REGION_C_ID = @REGION_C_ID ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@REGION_C_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        bolgeBilgileri.Add(new BOLGE()
                        {
                            REGION_ID = reader["REGION_ID"].milToInt32(),
                            REGION_NAME = reader["REGION_NAME"].milToString(),
                            REGION_STATUS_NAME = reader["STATUS_NAME"].milToString()
                        });
                    }

                    con.Close();
                }
                return bolgeBilgileri;
            }
        }
        internal BOLGE GetById(int id)
        {
            BOLGE bolge = new BOLGE();
            if (id > 0)
            {
                using (con = new SqlConnection(connectionString))
                {
                    string sqlQuery = $@"SELECT * FROM  dbo.TREGION INNER JOIN  dbo.TSTATUS ON dbo.TREGION.REGION_STATUS = dbo.TSTATUS.STATUS_ID WHERE REGION_ID = @REGION_ID";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@REGION_ID", id);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            bolge = new BOLGE()
                            {
                                REGION_ID = reader["REGION_ID"].milToInt32(),
                                REGION_NAME = reader["REGION_NAME"].milToString(),
                                REGION_CREATED_DATE = reader["REGION_CREATED_DATE"].milToDateTime(),
                                REGION_CREATED_USER_ID = reader["REGION_CREATED_USER_ID"].milToInt32(),
                                REGION_EDITED_DATE = reader["REGION_EDITED_DATE"].milToDateTime(),
                                REGION_EDITED_USER_ID = reader["REGION_EDITED_USER_ID"].milToInt32(),
                                REGION_DESCRIPTION = reader["REGION_DESCRIPTION"].milToString(),
                                REGION_C_ID = reader["REGION_ID"].milToInt32(),
                                REGION_STATUS_NAME = reader["STATUS_NAME"].milToString(),
                                REGION_STATUS = reader["REGION_STATUS"].milToInt32()
                            };
                        }
                        else
                            bolge = null;

                        con.Close();
                    }
                }
            }
            else
                bolge = null;

            return bolge;
        }
        internal BOLGE Insert(BOLGE bolge)
        {
            BOLGE insertedKategori = new BOLGE();
            int id;

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TREGION 
                ( REGION_NAME,REGION_STATUS, REGION_C_ID, REGION_DESCRIPTION,REGION_CREATED_DATE, REGION_CREATED_USER_ID ) 
                VALUES (@REGION_NAME, @REGION_STATUS, @REGION_C_ID, @REGION_DESCRIPTION, @REGION_CREATED_DATE, @REGION_CREATED_USER_ID);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@REGION_NAME", bolge.REGION_NAME.milToString());
                    cmd.Parameters.AddWithValue("@REGION_STATUS", 1);
                    cmd.Parameters.AddWithValue("@REGION_C_ID", bolge.REGION_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@REGION_DESCRIPTION", bolge.REGION_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@REGION_CREATED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@REGION_CREATED_USER_ID", bolge.REGION_CREATED_USER_ID.milToInt32());

                    id = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            insertedKategori = GetById(id);
            return insertedKategori;
        }
        internal BOLGE Update(BOLGE bolge)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"UPDATE TREGION SET
                REGION_NAME = @REGION_NAME, REGION_STATUS = @REGION_STATUS, REGION_C_ID = @REGION_C_ID,  REGION_DESCRIPTION = @REGION_DESCRIPTION, REGION_EDITED_DATE = @REGION_EDITED_DATE, REGION_EDITED_USER_ID = @REGION_EDITED_USER_ID WHERE REGION_ID = @REGION_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@REGION_NAME", bolge.REGION_NAME.milToString());
                    cmd.Parameters.AddWithValue("@REGION_STATUS", bolge.REGION_STATUS.milToInt32());
                    cmd.Parameters.AddWithValue("@REGION_C_ID", bolge.REGION_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@REGION_DESCRIPTION", bolge.REGION_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@REGION_EDITED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@REGION_EDITED_USER_ID", bolge.REGION_EDITED_USER_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@REGION_ID", bolge.REGION_ID.milToInt32());

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            }

            bolge = GetById(bolge.REGION_ID);
            return bolge;
        }
        internal bool Delete(int id)
        {
            bool result = false;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"DELETE FROM TREGION WHERE REGION_ID = @REGION_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@REGION_ID", id);

                    cmd.ExecuteNonQuery();
                    result = true;

                    con.Close();
                }
            }
            return result;
        }
    }
}