using exorderproject_api.Helpers;
using exorderproject_core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace exorderproject_api.DataAccess
{
    public class GarsonDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        EncryptionAndDecryptionHelper EncryptionAndDecryption = new EncryptionAndDecryptionHelper();


        internal List<SiparisData> HazirSiparis(int c_id)
        {
            List<SiparisData> siparisBilgileri = new List<SiparisData>();
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"SELECT * FROM  dbo.TORDERS  INNER JOIN  dbo.TTABLE ON dbo.TORDERS.ORDER_TABLE_ID = dbo.TTABLE.TABLE_ID INNER JOIN  dbo.TORDERSTATUS ON dbo.TORDERS.ORDER_STATUS_ID = dbo.TORDERSTATUS.ORDER_STATUS_ID WHERE (SIPARIS_DURUM_ID=0 or SIPARIS_DURUM_ID=2) and SIPARIS_SIRKET_ID = @c_id";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@c_id", c_id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        
                    }

                    con.Close();
                }
                return siparisBilgileri;
            }
        }
    }
}