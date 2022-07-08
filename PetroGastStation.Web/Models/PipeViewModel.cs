using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PetroGastStation.Web.Models
{
    public class PipeViewModel
    {
        public int Rows { get; set; }
        public string TAR { get; set; }
        [Display(Name = "Remision")]
        public string Remision { get; set; }
        [Display(Name = "Factura")]
        public string Factura { get; set; }
        public string Fecha { get; set; }
        [Display(Name = "Fecha Vencimiento")]
        public string Vencimiento { get; set; }
        [Display(Name = "Combustible")]
        public string Combustible { get; set; }
       
        [Display(Name = "Lts.Recibidos")]
        public string LtsRecibidos { get; set; }
        [Display(Name = "Litros")]
        public string Litros { get; set; }
        [Display(Name = "Subtotal")]
        public string Subtotal { get; set; }
        
        [Display(Name = "IVA")]
        public string IVA { get; set; }

        public string IEPS { get; set; }
        [Display(Name = "Flete")]
        public string Flete { get; set; }

        [Display(Name = "IVA Flete")]
        public string IVAFlete { get; set; }
        [Display(Name = "Reta")]
        public string Ret { get; set; }
        [Display(Name = "Desc")]
        public string Desc { get; set; }
        public string Total { get; set; }
        public StationViewModel Station { get; set; }
        [Display(Name = "Documents")]
        public IFormFile ImageFile { get; set; }
    }
}
