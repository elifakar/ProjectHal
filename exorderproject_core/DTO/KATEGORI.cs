using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class KATEGORI
    {
        public int CATEGORY_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CATEGORY_DESCRIPTION { get; set; }
        public string CATEGORY_IMG { get; set; }
        public int CATEGORY_C_ID { get; set; }
        public int CATEGORY_STATUS { get; set; }
        public string STATUS_NAME { get; set; }
        public DateTime CATEGORY_CREATED_DATE { get; set; }
        public int CATEGORY_CREATED_USER_ID { get; set; }
        public DateTime CATEGORY_EDITED_DATE { get; set; }
        public int CATEGORY_EDITED_USER_ID { get; set; }
        public string CATEGORY_STATUS_NAME { get; set; }
    }
}
