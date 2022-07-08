using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Models
{
    public class PriceSimpleReportViewModel
    {
        [Display(Name = "No. Permiso")]
        public string permiso { get; set; }
        [Display(Name = "Razon Social")]
        public string nombre { get; set; }
        public string ERP { get; set; }
        [Display(Name = "Precio Regular")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal precioRegular { get; set; }
        [Display(Name = "Precio Premium")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal precioPremium { get; set; }
        [Display(Name = "Precio Diesel")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal precioDiesel { get; set; }
    }
}
