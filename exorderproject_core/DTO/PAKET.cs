using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class PAKET
    {
        public int ID { get; set; }
        public string ADI { get; set; }
        public string IKON { get; set; }
        public string ACIKLAMA { get; set; }
        public int DURUM_ID { get; set; }
        public DateTime CREATED_AT { get; set; }
        public int CREATED_BY { get; set; }
        public decimal FIYAT { get; set; }
    }
}
