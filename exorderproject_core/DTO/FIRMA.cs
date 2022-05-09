using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class FIRMA
    {
        public int ID { get; set; }
        public string ADI { get; set; }
        public string ADI_KISA { get; set; }
        public string TEL { get; set; }
        public string EMAIL { get; set; }
        public string ADRES { get; set; }
        public string VERGI_DAIRESI { get; set; }
        public string VERGI_NO { get; set; }
        public string LOGO { get; set; }
        public int DURUM_ID { get; set; }
        public int SORUMLU_ID { get; set; }
        public DateTime CREATED_AT { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime EDITED_AT { get; set; }
        public int EDITED_BY { get; set; }
    }
}
