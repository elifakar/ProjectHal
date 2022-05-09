using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core
{
    public class ApiResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool Status { get; set; }
    }
}
