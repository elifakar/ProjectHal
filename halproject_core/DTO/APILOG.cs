using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace halproject_core.DTO
{
    public class APILOG
    {
        public int ID { get; set; }
        public string REQ_IP { get; set; }
        public string REQ_METHOD { get; set; }
        public string REQ_URL { get; set; }
        public DateTime REQ_DATE { get; set; }
        public bool RES_STATUS { get; set; }
        public int RES_STATUS_CODE { get; set; }
        public string RES_MESSAGE { get; set; }
        public string RES_R { get; set; }
        public DateTime RES_DATE { get; set; }
    }
}
