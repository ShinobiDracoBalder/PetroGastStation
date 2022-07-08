using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetroGastStation.Web.Models
{
    public class StationGasViewModel
    {
        [JsonProperty("id_precio")]
        public string IdPrecio { get; set; }

        [JsonProperty("permiso")]
        [Display(Name = "No Permiso")]
        public string Permiso { get; set; }

        [JsonProperty("nombre")]
        [Display(Name = "Razon Social")]
        public string Nombre { get; set; }

        [JsonProperty("RFC")]
        public string RFC { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("municipio")]
        public string Municipio { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("precioRegular")]
        [Display(Name = "Precio Regular")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal PrecioRegular { get; set; }

        [JsonProperty("actualizadoRegular")]
        public string ActualizadoRegular { get; set; }

        [JsonProperty("AtipicoRegular")]
        public string AtipicoRegular { get; set; }

        [JsonProperty("precioPremium")]
        [Display(Name = "Precio Premium")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal PrecioPremium { get; set; }

        [JsonProperty("actualizadoPremium")]
        public string ActualizadoPremium { get; set; }

        [JsonProperty("AtipicoPremium")]
        public string AtipicoPremium { get; set; }

        [JsonProperty("precioDiesel")]
        [Display(Name = "Precio Diesel")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal PrecioDiesel { get; set; }

        [JsonProperty("actualizadoDiesel")]
        public string ActualizadoDiesel { get; set; }

        [JsonProperty("AtipicoDiesel")]
        public string AtipicoDiesel { get; set; }

        [JsonProperty("fechaReporte")]
        public string FechaReporte { get; set; }
        [Display(Name = "No")]
        public int MainPetro { get; set; }

        public int GasEstacion_Id { get; set; }

        [Display(Name = "Precio Regular")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SugeridoPrecioRegular { get; set; }
        [Display(Name = "Precio Premium")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SugeridoPrecioPremium { get; set; }
        [Display(Name = "Precio Diesel")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SugeridoPrecioDiesel { get; set; }

        [Display(Name = "Competencia Directa Precio Regular")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal DirectCompetitionRegularPrice { get; set; }

        [Display(Name = "Competencia Directa Precio Premium")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal DirectCompetitionPremiumPrice { get; set; }

        [Display(Name = "Competencia Directa Precio Diesel")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal DirectCompetitionDieselPrice { get; set; }
    }
}
