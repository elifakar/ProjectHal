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
    public class MenuUrunDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        EncryptionAndDecryptionHelper EncryptionAndDecryption = new EncryptionAndDecryptionHelper();

        internal List<URUN> GetAll(int id)
        {
            List<URUN> urunBilgileri = new List<URUN>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM  dbo.TPRODUCT INNER JOIN dbo.TCATEGORY ON dbo.TPRODUCT.PRODUCT_CATEGORY_ID = dbo.TCATEGORY.CATEGORY_ID INNER JOIN  dbo.TSTATUS ON dbo.TPRODUCT.PRODUCT_STATUS = dbo.TSTATUS.STATUS_ID WHERE PRODUCT_C_ID = @PRODUCT_C_ID ";
                
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@PRODUCT_C_ID", id);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        urunBilgileri.Add(new URUN()
                        {
                            PRODUCT_ID = reader["PRODUCT_ID"].milToInt32(),
                            PRODUCT_NAME = reader["PRODUCT_NAME"].milToString(),
                            PRODUCT_ORDERNUMBER = reader["PRODUCT_ORDERNUMBER"].milToString(),
                            PRODUCT_QUANTITY=reader["PRODUCT_QUANTITY"].milToInt32(),
                            PRODUCT_PRICE = reader["PRODUCT_PRICE"].milToDecimal(),
                            CATEGORY_NAME = reader["CATEGORY_NAME"].milToString(),
                            PRODUCT_STATUS_NAME = reader["STATUS_NAME"].milToString(),
                            PRODUCT_C_ID = reader["PRODUCT_C_ID"].milToInt32(),
                            PRODUCT_STATUS = reader["PRODUCT_STATUS"].milToInt32(),
                            PRODUCT_IMG = reader["PRODUCT_IMG"].milToString()
                        });
                    }

                    con.Close();
                }
                return urunBilgileri;
            }
        }
        internal URUN GetById(int id)
        {
            URUN urun = new URUN();
            if (id > 0)
            {
                using (con = new SqlConnection(connectionString))
                {
                    string sqlQuery = $@"SELECT * FROM  dbo.TPRODUCT INNER JOIN dbo.TCATEGORY ON dbo.TPRODUCT.PRODUCT_CATEGORY_ID = dbo.TCATEGORY.CATEGORY_ID INNER JOIN  dbo.TSTATUS ON dbo.TPRODUCT.PRODUCT_STATUS = dbo.TSTATUS.STATUS_ID WHERE PRODUCT_ID = @PRODUCT_ID";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@PRODUCT_ID", id);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            urun = new URUN()
                            {
                                PRODUCT_ID = reader["PRODUCT_ID"].milToInt32(),
                                PRODUCT_NAME = reader["PRODUCT_NAME"].milToString(),
                                PRODUCT_CATEGORY_ID = reader["PRODUCT_CATEGORY_ID"].milToInt32(),
                                PRODUCT_IMG = reader["PRODUCT_IMG"].milToString(),
                                PRODUCT_QUANTITY = reader["PRODUCT_QUANTITY"].milToInt32(),
                                PRODUCT_PRICE = reader["PRODUCT_PRICE"].milToDecimal(),
                                PRODUCT_ORDERNUMBER = reader["PRODUCT_ORDERNUMBER"].milToString(),
                                PRODUCT_STATUS = reader["PRODUCT_STATUS"].milToInt32(),
                                PRODUCT_CREATED_DATE = reader["PRODUCT_CREATED_DATE"].milToDateTime(),
                                PRODUCT_CREATED_USER_ID = reader["PRODUCT_CREATED_USER_ID"].milToInt32(),
                                PRODUCT_EDITED_DATE = reader["PRODUCT_EDITED_DATE"].milToDateTime(),
                                PRODUCT_EDITED_USER_ID = reader["PRODUCT_EDITED_USER_ID"].milToInt32(),
                                PRODUCT_DESCRIPTION = reader["PRODUCT_DESCRIPTION"].milToString(),
                                CATEGORY_NAME = reader["CATEGORY_NAME"].milToString(),
                                PRODUCT_C_ID=reader["PRODUCT_C_ID"].milToInt32(),
                                PRODUCT_STATUS_NAME = reader["STATUS_NAME"].milToString()
                            };
                        }
                        else
                            urun = null;

                        con.Close();
                    }
                }
            }
            else
                urun = null;

            return urun;
        }
        internal URUN Insert(URUN urun)
        {
            URUN insertedUrun = new URUN();
            int id;

            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TPRODUCT 
                ( PRODUCT_NAME, PRODUCT_QUANTITY, PRODUCT_PRICE, PRODUCT_ORDERNUMBER,PRODUCT_STATUS, PRODUCT_C_ID, PRODUCT_CATEGORY_ID,PRODUCT_IMG, PRODUCT_DESCRIPTION,PRODUCT_CREATED_DATE, PRODUCT_CREATED_USER_ID ) 
                VALUES ( @PRODUCT_NAME, @PRODUCT_QUANTITY, @PRODUCT_PRICE,  @PRODUCT_ORDERNUMBER,@PRODUCT_STATUS, @PRODUCT_C_ID, @PRODUCT_CATEGORY_ID, @PRODUCT_IMG,@PRODUCT_DESCRIPTION, @PRODUCT_CREATED_DATE, @PRODUCT_CREATED_USER_ID);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRODUCT_NAME", urun.PRODUCT_NAME.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_STATUS", 1);
                    cmd.Parameters.AddWithValue("@PRODUCT_QUANTITY", urun.PRODUCT_QUANTITY.milToInt32()); 
                    cmd.Parameters.AddWithValue("@PRODUCT_PRICE", urun.PRODUCT_PRICE.milToDecimal());
                    cmd.Parameters.AddWithValue("@PRODUCT_ORDERNUMBER", urun.PRODUCT_ORDERNUMBER.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_C_ID", urun.PRODUCT_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_CATEGORY_ID", urun.PRODUCT_CATEGORY_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_DESCRIPTION", urun.PRODUCT_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_IMG", urun.PRODUCT_IMG.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_CREATED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@PRODUCT_CREATED_USER_ID", urun.PRODUCT_CREATED_USER_ID.milToInt32());
                   
                    id = cmd.ExecuteScalar().milToInt32();

                    con.Close();
                }
            }
            insertedUrun = GetById(id);
            return insertedUrun;
        }
        internal URUN Update(URUN urun)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"UPDATE TPRODUCT SET
                PRODUCT_NAME = @PRODUCT_NAME, PRODUCT_QUANTITY = @PRODUCT_QUANTITY, PRODUCT_STATUS = @PRODUCT_STATUS, PRODUCT_C_ID = @PRODUCT_C_ID, PRODUCT_CATEGORY_ID = @PRODUCT_CATEGORY_ID, PRODUCT_DESCRIPTION = @PRODUCT_DESCRIPTION, PRODUCT_EDITED_DATE = @PRODUCT_EDITED_DATE, PRODUCT_EDITED_USER_ID = @PRODUCT_EDITED_USER_ID WHERE PRODUCT_ID = @PRODUCT_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRODUCT_NAME", urun.PRODUCT_NAME.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_QUANTITY", urun.PRODUCT_QUANTITY.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_STATUS", urun.PRODUCT_STATUS.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_C_ID", urun.PRODUCT_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_CATEGORY_ID", urun.PRODUCT_CATEGORY_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_DESCRIPTION", urun.PRODUCT_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_EDITED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@PRODUCT_EDITED_USER_ID", urun.PRODUCT_EDITED_USER_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_ID", urun.PRODUCT_ID.milToInt32());

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            }

            urun = GetById(urun.PRODUCT_ID);
            return urun;
        }
        internal bool Delete(int id)
        {
            bool result = false;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"DELETE FROM TPRODUCT WHERE PRODUCT_ID = @PRODUCT_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRODUCT_ID", id);

                    cmd.ExecuteNonQuery();
                    result = true;

                    con.Close();
                }
            }
            return result;
        }
    }
}