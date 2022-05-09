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
    public class CalisanDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        EncryptionAndDecryptionHelper EncryptionAndDecryption = new EncryptionAndDecryptionHelper();

        internal List<CALISAN> GetAll(int id)
        {
            List<CALISAN> calisanBilgileri = new List<CALISAN>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM  dbo.TEMPLOYEES  INNER JOIN  dbo.TSTATUS ON dbo.TEMPLOYEES.EMPLOYEES_STATUS = dbo.TSTATUS.STATUS_ID WHERE EMPLOYEES_C_ID = @EMPLOYEES_C_ID ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@EMPLOYEES_C_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        calisanBilgileri.Add(new CALISAN()
                        {
                            EMPLOYEES_ID = reader["EMPLOYEES_ID"].milToInt32(),
                            EMPLOYEES_NAME = reader["EMPLOYEES_NAME"].milToString(),
                            EMPLOYEES_STATUS_NAME = reader["STATUS_NAME"].milToString()
                        });
                    }

                    con.Close();
                }
                return calisanBilgileri;
            }
        }
        internal CALISAN GetById(int id)
        {
            CALISAN calisan = new CALISAN();
            if (id > 0)
            {
                using (con = new SqlConnection(connectionString))
                {
                    string sqlQuery = $@"SELECT * FROM  dbo.TEMPLOYEES INNER JOIN  dbo.TSTATUS ON dbo.TEMPLOYEES.EMPLOYEES_STATUS = dbo.TSTATUS.STATUS_ID WHERE EMPLOYEES_ID = @EMPLOYEES_ID";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@EMPLOYEES_ID", id);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            calisan = new CALISAN()
                            {
                                EMPLOYEES_ID = reader["EMPLOYEES_ID"].milToInt32(),
                                EMPLOYEES_NAME = reader["EMPLOYEES_NAME"].milToString(),
                                EMPLOYEES_CREATED_DATE = reader["EMPLOYEES_CREATED_DATE"].milToDateTime(),
                                EMPLOYEES_CREATED_USER_ID = reader["EMPLOYEES_CREATED_USER_ID"].milToInt32(),
                                EMPLOYEES_EDITED_DATE = reader["EMPLOYEES_EDITED_DATE"].milToDateTime(),
                                EMPLOYEES_EDITED_USER_ID = reader["EMPLOYEES_EDITED_USER_ID"].milToInt32(),
                                EMPLOYEES_DESCRIPTION = reader["EMPLOYEES_DESCRIPTION"].milToString(),
                                EMPLOYEES_C_ID = reader["EMPLOYEES_ID"].milToInt32(),
                                EMPLOYEES_STATUS_NAME = reader["STATUS_NAME"].milToString(),
                                EMPLOYEES_STATUS = reader["EMPLOYEES_STATUS"].milToInt32()
                            };
                        }
                        else
                            calisan = null;

                        con.Close();
                    }
                }
            }
            else
                calisan = null;

            return calisan;
        }
        internal CALISAN Insert(CALISAN calisan)
        {
            CALISAN insertedKategori = new CALISAN();
            int id;

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TEMPLOYEES 
                ( EMPLOYEES_NAME,EMPLOYEES_STATUS, EMPLOYEES_C_ID, EMPLOYEES_DESCRIPTION,EMPLOYEES_CREATED_DATE, EMPLOYEES_CREATED_USER_ID ) 
                VALUES (@EMPLOYEES_NAME, @EMPLOYEES_STATUS, @EMPLOYEES_C_ID, @EMPLOYEES_DESCRIPTION, @EMPLOYEES_CREATED_DATE, @EMPLOYEES_CREATED_USER_ID);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@EMPLOYEES_NAME", calisan.EMPLOYEES_NAME.milToString());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_STATUS", 1);
                    cmd.Parameters.AddWithValue("@EMPLOYEES_C_ID", calisan.EMPLOYEES_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_DESCRIPTION", calisan.EMPLOYEES_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_CREATED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_CREATED_USER_ID", calisan.EMPLOYEES_CREATED_USER_ID.milToInt32());

                    id = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            insertedKategori = GetById(id);
            return insertedKategori;
        }
        internal CALISAN Update(CALISAN calisan)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"UPDATE TEMPLOYEES SET
                EMPLOYEES_NAME = @EMPLOYEES_NAME, EMPLOYEES_STATUS = @EMPLOYEES_STATUS, EMPLOYEES_C_ID = @EMPLOYEES_C_ID,  EMPLOYEES_DESCRIPTION = @EMPLOYEES_DESCRIPTION, EMPLOYEES_EDITED_DATE = @EMPLOYEES_EDITED_DATE, EMPLOYEES_EDITED_USER_ID = @EMPLOYEES_EDITED_USER_ID WHERE EMPLOYEES_ID = @EMPLOYEES_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@EMPLOYEES_NAME", calisan.EMPLOYEES_NAME.milToString());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_STATUS", calisan.EMPLOYEES_STATUS.milToInt32());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_C_ID", calisan.EMPLOYEES_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_DESCRIPTION", calisan.EMPLOYEES_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_EDITED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_EDITED_USER_ID", calisan.EMPLOYEES_EDITED_USER_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@EMPLOYEES_ID", calisan.EMPLOYEES_ID.milToInt32());

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            }

            calisan = GetById(calisan.EMPLOYEES_ID);
            return calisan;
        }
        internal bool Delete(int id)
        {
            bool result = false;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"DELETE FROM TEMPLOYEES WHERE EMPLOYEES_ID = @EMPLOYEES_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@EMPLOYEES_ID", id);

                    cmd.ExecuteNonQuery();
                    result = true;

                    con.Close();
                }
            }
            return result;
        }
    }
}