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
    public class AltBolgeDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        EncryptionAndDecryptionHelper EncryptionAndDecryption = new EncryptionAndDecryptionHelper();

        internal List<ALTBOLGE> GetAll(int id)
        {
            List<ALTBOLGE> altbolgeBilgileri = new List<ALTBOLGE>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM  dbo.TREGIONS INNER JOIN  dbo.TREGION ON dbo.TREGIONS.REGIONS_ID = dbo.TREGION.REGION_ID INNER JOIN  dbo.TSTATUS ON dbo.TREGIONS.REGIONS_STATUS = dbo.TSTATUS.STATUS_ID WHERE REGIONS_C_ID = @REGIONS_C_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@REGIONS_C_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        altbolgeBilgileri.Add(new ALTBOLGE()
                        {
                            REGIONS_ID = reader["REGIONS_ID"].milToInt32(),
                            REGIONS_NAME = reader["REGIONS_NAME"].milToString(),
                            REGIONS_STATUS_NAME = reader["STATUS_NAME"].milToString()
                        });
                    }

                    con.Close();
                }
                return altbolgeBilgileri;
            }
        }
        internal ALTBOLGE GetById(int id)
        {
            ALTBOLGE altbolge = new ALTBOLGE();
            if (id > 0)
            {
                using (con = new SqlConnection(connectionString))
                {
                    string sqlQuery = $@"SELECT * FROM  dbo.TREGIONS INNER JOIN  dbo.TREGION ON dbo.TREGIONS.REGIONS_ID = dbo.TREGION.REGION_ID INNER JOIN  dbo.TSTATUS ON dbo.TREGIONS.REGIONS_STATUS = dbo.TSTATUS.STATUS_ID WHERE REGIONS_ID = @REGIONS_ID";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@REGIONS_ID", id);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            altbolge = new ALTBOLGE()
                            {
                                REGIONS_ID = reader["REGIONS_ID"].milToInt32(),
                                REGIONS_NAME = reader["REGIONS_NAME"].milToString(),
                                REGIONS_CREATED_DATE = reader["REGIONS_CREATED_DATE"].milToDateTime(),
                                REGIONS_CREATED_USER_ID = reader["REGIONS_CREATED_USER_ID"].milToInt32(),
                                REGIONS_EDITED_DATE = reader["REGIONS_EDITED_DATE"].milToDateTime(),
                                REGIONS_EDITED_USER_ID = reader["REGIONS_EDITED_USER_ID"].milToInt32(),
                                REGIONS_DESCRIPTION = reader["REGIONS_DESCRIPTION"].milToString(),
                                REGIONS_C_ID = reader["REGIONS_C_ID"].milToInt32(),
                                REGIONS_STATUS_NAME = reader["STATUS_NAME"].milToString(),
                                REGIONS_STATUS = reader["REGIONS_STATUS"].milToInt32(),
                                REGIONS_REGION_ID=reader["REGIONS_REGION_ID"].milToInt32(),
                                REGIONS_REGION_NAME = reader["REGION_NAME"].milToString()
                            };
                        }
                        else
                            altbolge = null;

                        con.Close();
                    }
                }
            }
            else
                altbolge = null;

            return altbolge;
        }
        internal ALTBOLGE Insert(ALTBOLGE altbolge)
        {
            ALTBOLGE insertedKategori = new ALTBOLGE();
            int id;

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TREGIONS 
                ( REGIONS_NAME,REGIONS_STATUS, REGIONS_C_ID, REGIONS_DESCRIPTION,REGIONS_CREATED_DATE, REGIONS_CREATED_USER_ID ) 
                VALUES (@REGIONS_NAME, @REGIONS_STATUS, @REGIONS_C_ID, @REGIONS_DESCRIPTION, @REGIONS_CREATED_DATE, @REGIONS_CREATED_USER_ID);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@REGIONS_NAME", altbolge.REGIONS_NAME.milToString());
                    cmd.Parameters.AddWithValue("@REGIONS_STATUS", 1);
                    cmd.Parameters.AddWithValue("@REGIONS_C_ID", altbolge.REGIONS_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@REGIONS_DESCRIPTION", altbolge.REGIONS_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@REGIONS_CREATED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@REGIONS_CREATED_USER_ID", altbolge.REGIONS_CREATED_USER_ID.milToInt32());

                    id = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            insertedKategori = GetById(id);
            return insertedKategori;
        }
        internal ALTBOLGE Update(ALTBOLGE altbolge)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"UPDATE TREGIONS SET
                REGIONS_NAME = @REGIONS_NAME, REGIONS_STATUS = @REGIONS_STATUS, REGIONS_C_ID = @REGIONS_C_ID,  REGIONS_DESCRIPTION = @REGIONS_DESCRIPTION, REGIONS_EDITED_DATE = @REGIONS_EDITED_DATE, REGIONS_EDITED_USER_ID = @REGIONS_EDITED_USER_ID WHERE REGIONS_ID = @REGIONS_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@REGIONS_NAME", altbolge.REGIONS_NAME.milToString());
                    cmd.Parameters.AddWithValue("@REGIONS_STATUS", altbolge.REGIONS_STATUS.milToInt32());
                    cmd.Parameters.AddWithValue("@REGIONS_C_ID", altbolge.REGIONS_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@REGIONS_DESCRIPTION", altbolge.REGIONS_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@REGIONS_EDITED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@REGIONS_EDITED_USER_ID", altbolge.REGIONS_EDITED_USER_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@REGIONS_ID", altbolge.REGIONS_ID.milToInt32());

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            }

            altbolge = GetById(altbolge.REGIONS_ID);
            return altbolge;
        }
        internal bool Delete(int id)
        {
            bool result = false;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"DELETE FROM TREGIONS WHERE REGIONS_ID = @REGIONS_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@REGIONS_ID", id);

                    cmd.ExecuteNonQuery();
                    result = true;

                    con.Close();
                }
            }
            return result;
        }
    }
}