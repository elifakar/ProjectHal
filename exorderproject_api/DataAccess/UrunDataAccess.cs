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
    public class UrunDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        EncryptionAndDecryptionHelper EncryptionAndDecryption = new EncryptionAndDecryptionHelper();

        internal List<URUN> GetAll(int id)
        {
            List<URUN> urunBilgileri = new List<URUN>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM  dbo.TPRODUCT INNER JOIN dbo.TCATEGORY ON dbo.TPRODUCT.PRODUCT_CATEGORY_ID = dbo.TCATEGORY.CATEGORY_ID  INNER JOIN dbo.TUNIT ON dbo.TPRODUCT.PRODUCT_UNIT_ID = dbo.TUNIT.UNIT_ID  INNER JOIN  dbo.TSTATUS ON dbo.TPRODUCT.PRODUCT_STATUS = dbo.TSTATUS.STATUS_ID WHERE PRODUCT_C_ID = @PRODUCT_C_ID ";

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
                            PRODUCT_QUANTITY = reader["PRODUCT_QUANTITY"].milToInt32(),
                            PRODUCT_PRICE = reader["PRODUCT_PRICE"].milToDecimal(),
                            CATEGORY_NAME = reader["CATEGORY_NAME"].milToString(),
                            PRODUCT_STATUS_NAME = reader["STATUS_NAME"].milToString(),
                            PRODUCT_C_ID = reader["PRODUCT_C_ID"].milToInt32(),
                            PRODUCT_STATUS = reader["PRODUCT_STATUS"].milToInt32(),
                            PRODUCT_IMG = reader["PRODUCT_IMG"].milToString(),
                            PRODUCT_SUPPLIER_NAME = reader["PRODUCT_SUPPLIER_NAME"].milToString(),
                            PRODUCT_SUPPLIER_ADDRESS = reader["PRODUCT_SUPPLIER_ADDRESS"].milToString(),
                            PRODUCT_SUPPLIER_PHONE = reader["PRODUCT_SUPPLIER_PHONE"].milToString(),
                            PRODUCT_SUPPLIER_COMPANY = reader["PRODUCT_SUPPLIER_COMPANY"].milToString(),
                            PRODUCT_UNIT = reader["UNIT_NAME"].milToString()
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
                    string sqlQuery = $@"SELECT * FROM  dbo.TPRODUCT INNER JOIN dbo.TCATEGORY ON dbo.TPRODUCT.PRODUCT_CATEGORY_ID = dbo.TCATEGORY.CATEGORY_ID  INNER JOIN dbo.TUNIT ON dbo.TPRODUCT.PRODUCT_UNIT_ID = dbo.TUNIT.UNIT_ID INNER JOIN  dbo.TSTATUS ON dbo.TPRODUCT.PRODUCT_STATUS = dbo.TSTATUS.STATUS_ID WHERE PRODUCT_ID = @PRODUCT_ID";

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
                                PRODUCT_C_ID = reader["PRODUCT_C_ID"].milToInt32(),
                                PRODUCT_STATUS_NAME = reader["STATUS_NAME"].milToString(),
                                PRODUCT_SUPPLIER_NAME = reader["PRODUCT_SUPPLIER_NAME"].milToString(),
                                PRODUCT_SUPPLIER_ADDRESS = reader["PRODUCT_SUPPLIER_ADDRESS"].milToString(),
                                PRODUCT_SUPPLIER_PHONE = reader["PRODUCT_SUPPLIER_PHONE"].milToString(),
                                PRODUCT_SUPPLIER_COMPANY = reader["PRODUCT_SUPPLIER_COMPANY"].milToString(),
                                PRODUCT_UNIT = reader["UNIT_NAME"].milToString(),
                                PRODUCT_UNIT_ID = reader["PRODUCT_UNIT_ID"].milToInt32()
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
                ( PRODUCT_UNIT_ID,PRODUCT_NAME,PRODUCT_SUPPLIER_NAME,PRODUCT_SUPPLIER_ADDRESS,PRODUCT_SUPPLIER_PHONE,PRODUCT_SUPPLIER_COMPANY, PRODUCT_QUANTITY, PRODUCT_PRICE, PRODUCT_ORDERNUMBER,PRODUCT_STATUS, PRODUCT_C_ID, PRODUCT_CATEGORY_ID,PRODUCT_IMG, PRODUCT_DESCRIPTION,PRODUCT_CREATED_DATE, PRODUCT_CREATED_USER_ID ) 
                VALUES ( @PRODUCT_UNIT_ID,@PRODUCT_NAME,@PRODUCT_SUPPLIER_NAME,@PRODUCT_SUPPLIER_ADDRESS,@PRODUCT_SUPPLIER_PHONE,@PRODUCT_SUPPLIER_COMPANY, @PRODUCT_QUANTITY, @PRODUCT_PRICE,  @PRODUCT_ORDERNUMBER,@PRODUCT_STATUS, @PRODUCT_C_ID, @PRODUCT_CATEGORY_ID, @PRODUCT_IMG,@PRODUCT_DESCRIPTION, @PRODUCT_CREATED_DATE, @PRODUCT_CREATED_USER_ID);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRODUCT_NAME", urun.PRODUCT_NAME.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_STATUS", 1);
                    cmd.Parameters.AddWithValue("@PRODUCT_UNIT_ID", urun.PRODUCT_UNIT_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_QUANTITY", urun.PRODUCT_QUANTITY.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_PRICE", urun.PRODUCT_PRICE.milToDecimal());
                    cmd.Parameters.AddWithValue("@PRODUCT_ORDERNUMBER", urun.PRODUCT_ORDERNUMBER.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_C_ID", urun.PRODUCT_C_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_CATEGORY_ID", urun.PRODUCT_CATEGORY_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_DESCRIPTION", urun.PRODUCT_DESCRIPTION.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_IMG", urun.PRODUCT_IMG.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_CREATED_DATE", DateTime.Now.milToDateTime());
                    cmd.Parameters.AddWithValue("@PRODUCT_CREATED_USER_ID", urun.PRODUCT_CREATED_USER_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@PRODUCT_SUPPLIER_NAME", urun.PRODUCT_SUPPLIER_NAME.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_SUPPLIER_ADDRESS", urun.PRODUCT_SUPPLIER_ADDRESS.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_SUPPLIER_PHONE", urun.PRODUCT_SUPPLIER_PHONE.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_SUPPLIER_COMPANY", urun.PRODUCT_SUPPLIER_COMPANY.milToString());

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
                PRODUCT_NAME = @PRODUCT_NAME, PRODUCT_UNIT_ID=@PRODUCT_UNIT_ID, PRODUCT_QUANTITY = @PRODUCT_QUANTITY, PRODUCT_STATUS = @PRODUCT_STATUS, PRODUCT_C_ID = @PRODUCT_C_ID, PRODUCT_CATEGORY_ID = @PRODUCT_CATEGORY_ID, PRODUCT_DESCRIPTION = @PRODUCT_DESCRIPTION, PRODUCT_EDITED_DATE = @PRODUCT_EDITED_DATE, PRODUCT_EDITED_USER_ID = @PRODUCT_EDITED_USER_ID WHERE PRODUCT_ID = @PRODUCT_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRODUCT_UNIT_ID", urun.PRODUCT_UNIT_ID.milToInt32());
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
        internal URUN UpdateTedBilgileri(URUN urun)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"UPDATE TPRODUCT SET
                PRODUCT_SUPPLIER_NAME = @PRODUCT_SUPPLIER_NAME, PRODUCT_SUPPLIER_ADDRESS = @PRODUCT_SUPPLIER_ADDRESS, PRODUCT_SUPPLIER_PHONE = @PRODUCT_SUPPLIER_PHONE, PRODUCT_SUPPLIER_COMPANY = @PRODUCT_SUPPLIER_COMPANY, PRODUCT_EDITED_DATE = @PRODUCT_EDITED_DATE, PRODUCT_EDITED_USER_ID = @PRODUCT_EDITED_USER_ID WHERE PRODUCT_ID = @PRODUCT_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@PRODUCT_SUPPLIER_NAME", urun.PRODUCT_SUPPLIER_NAME.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_SUPPLIER_ADDRESS", urun.PRODUCT_SUPPLIER_ADDRESS.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_SUPPLIER_PHONE", urun.PRODUCT_SUPPLIER_PHONE.milToString());
                    cmd.Parameters.AddWithValue("@PRODUCT_SUPPLIER_COMPANY", urun.PRODUCT_SUPPLIER_COMPANY.milToString());
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
        internal List<STOK> StockInformation(int STOCK_PRODUCT_ID)
        {
            List<STOK> stokBilgileri = new List<STOK>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM  dbo.TSTOCK INNER JOIN dbo.TPRODUCT ON dbo.TSTOCK.STOCK_PRODUCT_ID = dbo.TPRODUCT.PRODUCT_ID INNER JOIN  dbo.TSTATUS ON dbo.TSTOCK.STOCK_STATUS_ID = dbo.TSTATUS.STATUS_ID WHERE STOCK_PRODUCT_ID = @STOCK_PRODUCT_ID ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@STOCK_PRODUCT_ID", STOCK_PRODUCT_ID.milToInt32());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        stokBilgileri.Add(new STOK()
                        {
                            STOCK_ID = reader["STOCK_ID"].milToInt32(),
                            STOCK_CURRENT_QUANTITY = reader["STOCK_CURRENT_QUANTITY"].milToDecimal(),
                            STOCK_QUANTITY = reader["STOCK_QUANTITY"].milToDecimal(),
                            STOCK_OLD_QUANTITY = reader["STOCK_OLD_QUANTITY"].milToDecimal(),
                            STOCK_PRODUCT_AVAILABILITY = reader["STOCK_PRODUCT_AVAILABILITY"].milToInt32(),
                            STOCK_TYPE = reader["STOCK_TYPE"].milToInt32(),
                            STOCK_STATUS_NAME = reader["STATUS_NAME"].milToString(),
                            STOCK_PRODUCT_ID = reader["STOCK_PRODUCT_ID"].milToInt32(),
                            STOCK_STATUS_ID = reader["STOCK_STATUS_ID"].milToInt32(),
                            STOCK_CREATED_DATE = reader["STOCK_CREATED_DATE"].milToDateTime()
                        });
                    }
                    con.Close();
                }
                return stokBilgileri;
            }
        }

        internal URUN UpdateStockDrop(STOK stok)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TSTOCK 
                (STOCK_PRODUCT_ID,STOCK_STATUS_ID,STOCK_CURRENT_QUANTITY,STOCK_QUANTITY,STOCK_OLD_QUANTITY,STOCK_PRODUCT_AVAILABILITY, STOCK_TYPE, STOCK_CREATED_DATE, STOCK_CREATED_USER_ID) 
                VALUES (@STOCK_PRODUCT_ID, @STOCK_STATUS_ID, @STOCK_CURRENT_QUANTITY, @STOCK_QUANTITY, @STOCK_OLD_QUANTITY, @STOCK_PRODUCT_AVAILABILITY, @STOCK_TYPE, @STOCK_CREATED_DATE, @STOCK_CREATED_USER_ID";

                decimal stokAdet = stok.STOCK_OLD_QUANTITY - stok.STOCK_QUANTITY;
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@STOCK_PRODUCT_ID", stok.STOCK_PRODUCT_ID);
                    cmd.Parameters.AddWithValue("@STOCK_STATUS_ID", 1);
                    cmd.Parameters.AddWithValue("@STOCK_CURRENT_QUANTITY", stokAdet);
                    cmd.Parameters.AddWithValue("@STOCK_QUANTITY", stok.STOCK_QUANTITY);
                    cmd.Parameters.AddWithValue("@STOCK_OLD_QUANTITY", stok.STOCK_OLD_QUANTITY);
                    cmd.Parameters.AddWithValue("@STOCK_CREATED_USER_ID", stok.STOCK_CREATED_USER_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@STOCK_PRODUCT_AVAILABILITY", stok.STOCK_PRODUCT_AVAILABILITY.milToInt32());
                    cmd.Parameters.AddWithValue("@STOCK_TYPE", 2); //1 stok ekle 2 stok düş
                    cmd.Parameters.AddWithValue("@STOCK_CREATED_DATE", DateTime.Now.milToDateTime());
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                string sqlQueryUpdate = $@"UPDATE TPRODUCT SET PRODUCT_QUANTITY = @PRODUCT_QUANTITY WHERE PRODUCT_ID = @PRODUCT_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQueryUpdate, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRODUCT_QUANTITY", stokAdet);
                    cmd.Parameters.AddWithValue("@PRODUCT_ID", stok.STOCK_PRODUCT_ID.milToInt32());
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            URUN urun = new URUN();
            urun = GetById(stok.STOCK_PRODUCT_ID);
            return urun;
        }
        internal URUN UpdateStockInsert(STOK stok)
        {
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TSTOCK 
                (STOCK_PRODUCT_ID,STOCK_STATUS_ID,STOCK_CURRENT_QUANTITY,STOCK_QUANTITY,STOCK_OLD_QUANTITY,STOCK_PRODUCT_AVAILABILITY, STOCK_TYPE, STOCK_CREATED_DATE, STOCK_CREATED_USER_ID) 
                VALUES (@STOCK_PRODUCT_ID, @STOCK_STATUS_ID, @STOCK_CURRENT_QUANTITY, @STOCK_QUANTITY, @STOCK_OLD_QUANTITY, @STOCK_PRODUCT_AVAILABILITY, @STOCK_TYPE, @STOCK_CREATED_DATE, @STOCK_CREATED_USER_ID";

                decimal stokAdet = stok.STOCK_OLD_QUANTITY + stok.STOCK_QUANTITY;
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@STOCK_PRODUCT_ID", stok.STOCK_PRODUCT_ID);
                    cmd.Parameters.AddWithValue("@STOCK_STATUS_ID", 1);
                    cmd.Parameters.AddWithValue("@STOCK_CURRENT_QUANTITY", stokAdet);
                    cmd.Parameters.AddWithValue("@STOCK_QUANTITY", stok.STOCK_QUANTITY);
                    cmd.Parameters.AddWithValue("@STOCK_OLD_QUANTITY", stok.STOCK_OLD_QUANTITY);
                    cmd.Parameters.AddWithValue("@STOCK_CREATED_USER_ID", stok.STOCK_CREATED_USER_ID.milToInt32());
                    cmd.Parameters.AddWithValue("@STOCK_PRODUCT_AVAILABILITY", stok.STOCK_PRODUCT_AVAILABILITY.milToInt32());
                    cmd.Parameters.AddWithValue("@STOCK_TYPE", 2); //1 stok ekle 2 stok düş
                    cmd.Parameters.AddWithValue("@STOCK_CREATED_DATE", DateTime.Now.milToDateTime());
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                string sqlQueryUpdate = $@"UPDATE TPRODUCT SET PRODUCT_QUANTITY = @PRODUCT_QUANTITY WHERE PRODUCT_ID = @PRODUCT_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQueryUpdate, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRODUCT_QUANTITY", stokAdet);
                    cmd.Parameters.AddWithValue("@PRODUCT_ID", stok.STOCK_PRODUCT_ID.milToInt32());
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            URUN urun = new URUN();
            urun = GetById(stok.STOCK_PRODUCT_ID);
            return urun;
        }

        internal bool StockDelete(int id)
        {
            bool result = false;
            using (con = new SqlConnection(connectionString))
            {
                //stok bilgisini bul
                string sqlQueryStok = $@"Select * FROM TSTOCK inner join TPRODUCT on TSTOCK.STOCK_PRODUCT_ID=TPRODUCT.PRODUCT_ID WHERE STOCK_ID = @STOCK_ID";
                decimal sonGirilenstokAdet = 0;
                decimal urunStokAdet = 0;
                using (SqlCommand cmd = new SqlCommand(sqlQueryStok, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@STOCK_ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        sonGirilenstokAdet = reader["STOCK_QUANTITY"].milToDecimal();
                        urunStokAdet = reader["PRODUCT_QUANTITY"].milToDecimal();
                    }

                    con.Close();
                }

                string sqlQuery = $@"DELETE FROM TSTOCK WHERE STOCK_ID = @STOCK_ID";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@STOCK_ID", id);

                    cmd.ExecuteNonQuery();
                    result = true;

                    con.Close();
                }

                //stok bilgisi düzenle
                decimal yeniStok = urunStokAdet - sonGirilenstokAdet;
                string sqlQueryUpdate = $@"UPDATE TPRODUCT SET PRODUCT_QUANTITY = @PRODUCT_QUANTITY WHERE PRODUCT_ID = @PRODUCT_ID";
                using (SqlCommand cmd = new SqlCommand(sqlQueryUpdate, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@PRODUCT_QUANTITY", yeniStok);
                    cmd.Parameters.AddWithValue("@PRODUCT_ID", id.milToInt32());
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return result;
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