using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class URUN
    {
        public int PRODUCT_ID { get; set; }
        public int? PRODUCT_C_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public decimal? PRODUCT_PRICE { get; set; }
        public int PRODUCT_QUANTITY { get; set; }
        public string PRODUCT_ORDERNUMBER { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string PRODUCT_STATUS_NAME { get; set; }
        public int? PRODUCT_STATUS { get; set; }
        public string PRODUCT_IMG { get; set; }
        public DateTime PRODUCT_CREATED_DATE { get; set; }
        public int PRODUCT_CREATED_USER_ID { get; set; }
        public int PRODUCT_EDITED_USER_ID { get; set; }
        public DateTime PRODUCT_EDITED_DATE { get; set; }
        public string PRODUCT_DESCRIPTION { get; set; }
        public int PRODUCT_CATEGORY_ID { get; set; }
        public string PRODUCT_SUPPLIER_NAME { get; set; }
        public string PRODUCT_SUPPLIER_ADDRESS { get; set; }
        public string PRODUCT_SUPPLIER_PHONE { get; set; }
        public string PRODUCT_SUPPLIER_COMPANY { get; set; }
        public int PRODUCT_UNIT_ID { get; set; }
        public string PRODUCT_UNIT { get; set; }
    }
}
