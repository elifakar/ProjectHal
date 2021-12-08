using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace halproject_core.DTO
{
    public class PASSWORDRESETTEMP
    {
        public int ID { get; set; }
        public int KULLANICI_ID { get; set; }
        public string GUID { get; set; }
        public DateTime SONTARIH { get; set; }
    }
}
