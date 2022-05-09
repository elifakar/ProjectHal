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
    public class KategoriDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        EncryptionAndDecryptionHelper EncryptionAndDecryption = new EncryptionAndDecryptionHelper();

        internal List<KATEGORI> GetAll(int id)
        {
            List<KATEGORI> kategoriBilgileri = new List<KATEGORI>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM  dbo.TCATEGORY  INNER JOIN  dbo.TSTATUS ON dbo.TCATEGORY.CATEGORY_STATUS = dbo.TSTATUS.STATUS_ID WHERE CATEGORY_C_ID = @CATEGORY_C_ID ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@CATEGORY_C_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        kategoriBilgileri.Add(new KATEGORI()
                        {
                            CATEGORY_ID = reader["CATEGORY_ID"].milToInt32(),
                            CATEGORY_NAME = reader["CATEGORY_NAME"].milToString(),
                            CATEGORY_IMG = reader["CATEGORY_IMG"].milToString(),
                            CATEGORY_STATUS_NAME=reader["STATUS_NAME"].milToString()
                        });
                    }

                    con.Close();
                }
                return kategoriBilgileri;
            }
        }
        internal KATEGORI GetById(int id)
        {
            KATEGORI kategori = new KATEGORI();
            if (id > 0)
            {
                using (con = new SqlConnection(connectionString))
                {
                    string sqlQuery = $@"SELECT * FROM  dbo.TCATEGORY INNER JOIN  dbo.TSTATUS ON dbo.TCATEGORY.CATEGORY_STATUS = dbo.TSTATUS.STATUS_ID WHERE CATEGORY_ID = @CATEGORY_ID";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@CATEGORY_ID", id);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            kategori = new KATEGORI()
                            {
                                CATEGORY_ID = reader["CATEGORY_ID"].milToInt32(),
                                CATEGORY_NAME = reader["CATEGORY_NAME"].milToString(),
                                CATEGORY_IMG = reader["CATEGORY_IMG"].milToString(),
                                CATEGORY_CREATED_DATE = reader["CATEGORY_CREATED_DATE"].milToDateTime(),
                                CATEGORY_CREATED_USER_ID = reader["CATEGORY_CREATED_USER_ID"].milToInt32(),
                                CATEGORY_EDITED_DATE = reader["CATEGORY_EDITED_DATE"].milToDateTime(),
                                CATEGORY_EDITED_USER_ID = reader["CATEGORY_EDITED_USER_ID"].milToInt32(),
                                CATEGORY_DESCRIPTION = reader["CATEGORY_DESCRIPTION"].milToString(),
                                CATEGORY_C_ID = reader["CATEGORY_C_ID"].milToInt32(),
                                CATEGORY_STATUS_NAME = reader["STATUS_NAME"].milToString(),
                                CATEGORY_STATUS=reader["CATEGORY_STATUS"].milToInt32()
                            };
                        }
                        else
                            kategori = null;

                        con.Close();
                    }
                }
            }
            else
                kategori = null;

            return kategori;
        }
        internal KATEGORI Insert(KATEGORI kategori)
        {
            KATEGORI insertedKategori = new KATEGORI();
            int id;

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TCATEGORY 
                ( CATEGORY_NAME,CATEGORY_STATUS, CATEGORY_C_ID, CATEGORY_IMG, CATEGORY_DESCRIPTION,CATEGORY_CREATED_DATE, CATEGORY_CREATED_USER_ID ) 
                VALUES (@CATEGORY_NAME, @CATEGORY_STATUS, @CATEGORY_C_ID, @CATEGORY_IMG,@CATEGORY_DESCRIPTION, @CATEGORY_CREATED_DATE, @CATEGORY_CREATED_USER_ID);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@CATEGORY_NAME", kategori.CATEGORY_NAME.milToString());
                    cmd.Parameters.AddWithValue("@CATEGORY_STATUS", 1);
                    cmd.Parameters.AddWithValue("@CATEGORY_C_ID", kategori.CATEGORY_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@CATEGORY_DESCRIPTION", kategori.CATEGORY_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@CATEGORY_IMG", kategori.CATEGORY_IMG.milToString());
                    cmd.Parameters.AddWithValue("@CATEGORY_CREATED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@CATEGORY_CREATED_USER_ID", kategori.CATEGORY_CREATED_USER_ID.milToInt32());

                    id = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            insertedKategori = GetById(id);
            return insertedKategori;
        }
        internal KATEGORI Update(KATEGORI kategori)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"UPDATE TCATEGORY SET
                CATEGORY_NAME = @CATEGORY_NAME, CATEGORY_STATUS = @CATEGORY_STATUS, CATEGORY_C_ID = @CATEGORY_C_ID,  CATEGORY_DESCRIPTION = @CATEGORY_DESCRIPTION, CATEGORY_EDITED_DATE = @CATEGORY_EDITED_DATE, CATEGORY_EDITED_USER_ID = @CATEGORY_EDITED_USER_ID WHERE CATEGORY_ID = @CATEGORY_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@CATEGORY_NAME", kategori.CATEGORY_NAME.milToString());
                    cmd.Parameters.AddWithValue("@CATEGORY_STATUS", kategori.CATEGORY_STATUS.milToInt32());
                    cmd.Parameters.AddWithValue("@CATEGORY_C_ID", kategori.CATEGORY_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@CATEGORY_DESCRIPTION", kategori.CATEGORY_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@CATEGORY_EDITED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@CATEGORY_EDITED_USER_ID", kategori.CATEGORY_EDITED_USER_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@CATEGORY_ID", kategori.CATEGORY_ID.milToInt32());

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            }

            kategori = GetById(kategori.CATEGORY_ID);
            return kategori;
        }
        internal bool Delete(int id)
        {
            bool result = false;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"DELETE FROM TCATEGORY WHERE CATEGORY_ID = @CATEGORY_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@CATEGORY_ID", id);

                    cmd.ExecuteNonQuery();
                    result = true;

                    con.Close();
                }
            }
            return result;
        }
    }
}