using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.DTO
{
    public class APIKULLANICILOG
    {
        public int ID { get; set; }
        public int KULLANICI_ID { get; set; }
        public DateTime TARIH { get; set; }
        public string TOKEN { get; set; }
        public DateTime SONTARIH { get; set; }
    }
}
