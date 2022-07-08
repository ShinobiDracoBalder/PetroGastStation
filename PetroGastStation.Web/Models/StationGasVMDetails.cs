using System.Collections.Generic;

namespace PetroGastStation.Web.Models
{
    public class StationGasVMDetails : StationGasViewModel
    {
        public List<StationGasViewModel> CompetitionDetails { get; set; }
    }
}
