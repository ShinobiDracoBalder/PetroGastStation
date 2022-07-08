namespace PetroGastStation.Web.DataAccess.IDBInterface
{
    public interface IUnitOfWork
    {
        IGasStationRepository GasStation { get; }
    }
}
