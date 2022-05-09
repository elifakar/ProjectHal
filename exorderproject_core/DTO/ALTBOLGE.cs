using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class ALTBOLGE
    {
        public int REGIONS_ID { get; set; }
        public string REGIONS_NAME { get; set; }
        public string REGIONS_DESCRIPTION { get; set; }
        public int REGIONS_C_ID { get; set; }
        public int REGIONS_CREATED_USER_ID { get; set; }
        public int REGIONS_EDITED_USER_ID { get; set; }
        public DateTime REGIONS_CREATED_DATE { get; set; }
        public DateTime REGIONS_EDITED_DATE { get; set; }
        public int REGIONS_STATUS { get; set; }
        public string REGIONS_STATUS_NAME { get; set; }
        public int REGIONS_REGION_ID { get; set; }
        public string REGIONS_REGION_NAME { get; set; }
    }
}
