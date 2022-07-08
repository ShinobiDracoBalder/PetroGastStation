using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetroGastStation.Web.Models
{
    public class CreatePost : ReponseModel
    {
        public string ImageCaption { set; get; }
        public string ImageDescription { set; get; }

        [Required(ErrorMessage = "Please enter file name")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Please select files")]
        public IFormFile File { set; get; }

        public List<StationGasViewModel> FullGasStations { get; set; }
    }
}
