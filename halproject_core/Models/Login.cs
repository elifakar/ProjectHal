using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace halproject_core.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Zorunlu Alan.")]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan.")]
        public string SIFRE { get; set; }
    }
}
