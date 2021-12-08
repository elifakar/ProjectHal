using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace halproject_core.DTO
{
    public class KULLANICI
    {
        public int ID { get; set; }
        public string KULLANICI_ADSOYAD { get; set; }
        public string KULLANICI_ADI { get; set; }
        public string EMAIL { get; set; }
        public string SIFRE { get; set; }
        public string TEL { get; set; }
        public int FIRMA_ID { get; set; }
        public int DURUM_ID { get; set; }
        public DateTime CREATED_AT { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime EDITED_AT { get; set; }
        public int EDITED_BY { get; set; }
    }
}
