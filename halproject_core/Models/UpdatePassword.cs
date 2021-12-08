using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace halproject_core.Models
{
    public class UpdatePassword
    {
        public int KULLANICI_ID { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        public string ESKISIFRE { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        public string YENISIFRE { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        public string YENISIFRE_TEKRAR { get; set; }
    }
}
