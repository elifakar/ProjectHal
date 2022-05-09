using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class CALISAN
    {
        public int EMPLOYEES_ID { get; set; }
        public string EMPLOYEES_NAME { get; set; }
        public string EMPLOYEES_TITLE { get; set; }
        public System.DateTime EMPLOYEES_BIRTHDAY { get; set; }
        public string EMPLOYEES_ADRESS { get; set; }
        public string EMPLOYEES_CITY { get; set; }
        public string EMPLOYEES_REGION { get; set; }
        public string EMPLOYEES_POSTAL_CODE { get; set; }
        public string EMPLOYEES_COUNTRY { get; set; }
        public string EMPLOYEES_PHONE_NUMBER { get; set; }
        public string EMPLOYEES_IMG { get; set; }
        public string EMPLOYEES_DESCRIPTION { get; set; }
        public int EMPLOYEES_C_ID { get; set; }
        public int EMPLOYEES_CREATED_USER_ID { get; set; }
        public DateTime EMPLOYEES_CREATED_DATE { get; set; }
        public int EMPLOYEES_EDITED_USER_ID { get; set; }
        public DateTime EMPLOYEES_EDITED_DATE { get; set; }
        public int EMPLOYEES_STATUS { get; set; }
        public string EMPLOYEES_STATUS_NAME { get; set; }
    }
}
