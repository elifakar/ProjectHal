using halproject_core.DTO;
using halproject_core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace halproject_api.DataAccess
{
    public class LogDataAccess
    {
        SqlConnection con;
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];

        internal async Task<APILOG> Insert(APILOG log)
        {
            APILOG insertedLog = log;
            using (con = new SqlConnection(connectionString))
            {
                string sqlQuery = $@"INSERT INTO TAPILOG 
                (APILOG_REQ_IP, APILOG_REQ_METHOD, APILOG_REQ_URL, APILOG_REQ_DATE, APILOG_RES_STATUS, APILOG_RES_STATUS_CODE, APILOG_RES_MESSAGE, APILOG_RES_DATE, APILOG_RES_R) 
                VALUES ( @APILOG_REQ_IP, @APILOG_REQ_METHOD, @APILOG_REQ_URL, @APILOG_REQ_DATE, @APILOG_RES_STATUS, @APILOG_RES_STATUS_CODE, @APILOG_RES_MESSAGE, @APILOG_RES_DATE, @APILOG_RES_R);  SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@APILOG_REQ_IP", log.REQ_IP.milToString());
                    cmd.Parameters.AddWithValue("@APILOG_REQ_METHOD", log.REQ_METHOD.milToString());
                    cmd.Parameters.AddWithValue("@APILOG_REQ_URL", log.REQ_URL.milToString());
                    cmd.Parameters.AddWithValue("@APILOG_REQ_DATE", log.REQ_DATE.milToDateTime());
                    cmd.Parameters.AddWithValue("@APILOG_RES_STATUS", log.RES_STATUS.milToBool());
                    cmd.Parameters.AddWithValue("@APILOG_RES_STATUS_CODE", log.RES_STATUS_CODE.milToInt32());
                    cmd.Parameters.AddWithValue("@APILOG_RES_MESSAGE", log.RES_MESSAGE.milToString());
                    cmd.Parameters.AddWithValue("@APILOG_RES_DATE", log.RES_DATE.milToDateTime());
                    cmd.Parameters.AddWithValue("@APILOG_RES_R", log.RES_R.milToString());

                    insertedLog.ID = (await cmd.ExecuteScalarAsync()).milToInt32();

                    con.Close();
                }
            }

            return insertedLog;
        }
    }
}