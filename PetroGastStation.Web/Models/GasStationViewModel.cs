using System.ComponentModel.DataAnnotations;

namespace PetroGastStation.Web.Models
{
    public class GasStationViewModel
    {
        public int GEstacionServicioId { get; set; }
        [Display(Name = "ESTACION")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string? RazonSocial { get; set; }
        [Display(Name = "RFC")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string? Rfc { get; set; }
        [Display(Name = "Numero Permiso")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string? NumeroPermiso { get; set; }
        [Display(Name = "No Permiso")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string? PermisoId { get; set; }
        [Display(Name = "Longitud")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string? Longitude { get; set; }
        [Display(Name = "Latitud")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string? Latitude { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        public int EstadoId { get; set; }
        public int StatusId { get; set; }
    }
}
