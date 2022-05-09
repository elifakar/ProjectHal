using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class FIRMAPAKET
    {
        public int ID { get; set; }
        public int FIRMA_ID { get; set; }
        public int PAKET_ID { get; set; }
        public int PAKETDURUM_ID { get; set; }
        public DateTime CREATED_AT { get; set; }
    }
}
