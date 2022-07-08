using System.Collections.Generic;

namespace PetroGastStation.Web.Models
{
    public class StationPipeUpdateAddViewModel
    {
        public List<StationPipeViewModel> AddDatabasePipe { get; set; }
        public List<StationPipeViewModel> ModDatabasePipe { get; set; }
        public bool IsSucceeded { get; set; }
        public string MessageSuccess { get; set; }

    }
}
