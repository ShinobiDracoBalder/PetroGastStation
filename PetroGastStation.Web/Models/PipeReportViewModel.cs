using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Models
{
    public class PipeReportViewModel
    {
        public int PipeReportId { get; set; }
        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }
        [Display(Name = "Numero Permiso")]
        public string NumeroPermiso { get; set; }
        public string Apelativo { get; set; }
        public int Rows { get; set; }
        public string TAR { get; set; }
        [Display(Name = "Remision")]
        public string Remision { get; set; }
        [Display(Name = "Factura")]
        public string Factura { get; set; }
        [Display(Name = "Fecha")]
        public string Fecha { get; set; }
        [Display(Name = "Vencimiento")]
        public string Vencimiento { get; set; }
        [Display(Name = "Combustible")]
        public string Combustible { get; set; }

        [Display(Name = "Lts.Recibidos")]
        public string LtsRecibidos { get; set; }
        [Display(Name = "Litros")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Litros { get; set; }
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
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Total { get; set; }
        public DateTime RegisterDate { get; set; }

        [Display(Name = "Costo por litro")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal CostoLitro => Total/Litros;
    }
}
