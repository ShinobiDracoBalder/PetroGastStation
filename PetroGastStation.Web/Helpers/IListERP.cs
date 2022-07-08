using PetroGastStation.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Helpers
{
    public interface IListERP
    {
        List<PipeERPViewModel> GetAllAsync();
    }
}
