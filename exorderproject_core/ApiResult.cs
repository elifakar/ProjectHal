using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core
{
    public class ApiResult<T>
    {
        public bool STATUS { get; set; }
        public int STATUS_CODE { get; set; }
        public string MESSAGE { get; set; }
        public DateTime DATE { get; set; }
        public T R { get; set; }
    }
}
