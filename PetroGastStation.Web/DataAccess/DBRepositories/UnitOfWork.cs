using PetroGastStation.Web.DataAccess.IDBInterface;

namespace PetroGastStation.Web.DataAccess.DBRepositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGasStationRepository GasStation { get; }

        public UnitOfWork(IGasStationRepository gasStationRepository)
        {
            GasStation = gasStationRepository;
        }
    }
}
