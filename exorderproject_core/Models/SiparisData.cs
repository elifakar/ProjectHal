using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exorderproject_core.Models
{
    public class SiparisDetay
    {
        public SiparisData siparis { get; set; }
        public List<SiparisDetayData> siparisDetay { get; set; }
    }
    public class SiparisData
    {
        public int SIPARIS_ID { get; set; }
        public string SIPARIS_NAME { get; set; }
        public DateTime? SIPARIS_TARIH { get; set; }
        public string MASA { get; set; }
        public int? MASA_NO { get; set; }
        public int? MASA_ID { get; set; }
        public string SIPARIS_DURUM { get; set; }
        public int? SIPARIS_DURUM_ID { get; set; }
        public int? SIPARIS_SIRKET_ID { get; set; }
        public string SIPARIS_GECENSURE { get; set; }
    }

    public class SiparisDetayData
    {
        public int SIPARISDETAY_ID { get; set; }
        public int? SIPARISDETAY_SIPARIS_ID { get; set; }
        public string SIPARISDETAY_DURUM { get; set; }
        public int? SIPARISDETAY_DURUM_ID { get; set; }
        public int? SIPARISDETAY_URUN_ID { get; set; }
        public string SIPARISDETAY_URUN { get; set; }
        public decimal? SIPARISDETAY_UCRET { get; set; }
        public decimal? SIPARISDETAY_TOPLAM_UCRET { get; set; }
        public int? SIPARISDETAY_ADET { get; set; }
        public decimal SIPARISDETAY_INDIRIM { get; set; }
    }
}
