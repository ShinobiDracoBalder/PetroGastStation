using System;
using System.ComponentModel.DataAnnotations;

namespace PetroGastStation.Web.Models
{
    public class StationPipeViewModel
    {
        public int Rows { get; set; }
        public string TAR { get; set; }
        [Display(Name = "Remision")]
        public string Remision { get; set; }
        [Display(Name = "Factura")]
        public string Factura { get; set; }
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime? Fecha { get; set; }
        [Display(Name = "Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime? Vencimiento { get; set; }
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
        [Display(Name = "Descuento")]
        public string Descuento { get; set; }
        public string Total { get; set; }
        public string GasStation { get; set; }
        public int Deleted { get; set; }
        public DateTime RegisterDate { get; set; }
        public int PipeReportId { get; set; }
        public string RazonSocial { get; set; }
        public string NumeroPermiso { get; set; }
        public string Apelativo { get; set; }
    }
}
