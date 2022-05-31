using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class STOK
    {
        public int STOCK_ID { get; set; }
        public int STOCK_PRODUCT_ID { get; set; }
        public int STOCK_STATUS_ID { get; set; }
        public decimal STOCK_CURRENT_QUANTITY { get; set; }
        public decimal STOCK_QUANTITY { get; set; }
        public decimal STOCK_OLD_QUANTITY { get; set; }
        public int STOCK_PRODUCT_AVAILABILITY { get; set; }
        public int STOCK_TYPE { get; set; }
        public DateTime STOCK_CREATED_DATE { get; set; }
        public int STOCK_CREATED_USER_ID { get; set; }
        public string STOCK_STATUS_NAME { get; set; }
        public int STOCK_PRODUCT_C_ID { get; set; }
    }
}