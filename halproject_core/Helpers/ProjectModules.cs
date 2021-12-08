using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace halproject_core.Helpers
{
    public static class ProjectModules
    {
        public static int milToInt32(this object str)
        {
            int rslt = 0;
            try
            {
                if (str != null)
                    int.TryParse(str.ToString(), out rslt);
            }
            catch
            {
                rslt = 0;
            }
            return rslt;
        }

        public static string milToString(this object str)
        {
            string rslt = "";
            try
            {
                if (str != null)
                    rslt = str.ToString();
            }
            catch
            {
                rslt = "";
            }
            return rslt;
        }

        public static DateTime milToDateTime(this object str)
        {
            DateTime rslt = DateTime.MinValue;
            try
            {
                DateTime.TryParse(str.ToString(), out rslt);
            }
            catch
            {
                rslt = DateTime.MinValue;
            }
            return rslt;
        }
        
        public static SqlDateTime milToSqlDateTime(this object str)
        {
            
            SqlDateTime rslt;
            if (object.ReferenceEquals(null, str))
            {
                rslt = SqlDateTime.Null;
            }
            else
            {
                try
                {
                     DateTime dt;
                     DateTime.TryParse(str.ToString(),out dt);

                    // Part to SqlDateTime then            
                    rslt = System.Data.SqlTypes.SqlDateTime.Parse(dt.ToString("MM-dd-yyyy"));
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return rslt;
        }
        
        public static decimal milToDecimal(this object str)
        {
            decimal rslt = 0;
            try
            {
                if (str != null)
                    decimal.TryParse(str.ToString(), out rslt);
            }
            catch
            {
                rslt = 0;
            }
            return rslt;
        }

        public static bool milToBool(this object str)
        {
            bool rslt = false;
            try
            {
                if (str != null)
                    bool.TryParse(str.ToString(), out rslt);
            }
            catch
            {
                rslt = false;
            }
            return rslt;
        }
    }
}
