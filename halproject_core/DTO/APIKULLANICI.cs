using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace halproject_core.DTO
{
    public class APIKULLANICI
    {
        public int ID { get; set; }
        public string KULLANICI_ADI { get; set; }
        public string SIFRE { get; set; }
        public int DURUM_ID { get; set; }
        public int CONTINUE_TIME { get; set; }
        public string TOKEN { get; set; }
    }
}
