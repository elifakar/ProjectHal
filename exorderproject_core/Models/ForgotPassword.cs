using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Zorunlu Alan.")]
        public string EMAIL { get; set; }
    }
}
