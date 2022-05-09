using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class BOLGE
    {
        public int REGION_ID { get; set; }
        public string REGION_NAME { get; set; }
        public string REGION_DESCRIPTION { get; set; }
        public int REGION_C_ID { get; set; }
        public int REGION_CREATED_USER_ID { get; set; }
        public int REGION_EDITED_USER_ID { get; set; }
        public DateTime REGION_CREATED_DATE { get; set; }
        public DateTime REGION_EDITED_DATE { get; set; }
        public int REGION_STATUS { get; set; }
        public string REGION_STATUS_NAME { get; set; }
    }
}
