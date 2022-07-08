using PetroGastStation.Common.Responses;
using PetroGastStation.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetroGastStation.Web.DataAccess.IDBInterface
{
    public interface IGasStationRepository : IGenericUnitofWorkRepository<GasStationViewModel>
    {
        Task<IEnumerable<GasStationViewModel>> GetGasStationByOrderNameAsync(string orderName, string SqlQuery);
        Task<IEnumerable<GasStationViewModel>> GetGasStationByNameAsync(int Id, string SqlQuery);
        Task<Response<GasStationViewModel>> GetGasStationByOnlyAsync(int Id, string SqlQuery);
        Task<Response<GasStationViewModel>> AllGasStationByAsync(string SqlQuery);
    }
}
