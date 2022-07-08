using PetroGastStation.Common.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetroGastStation.Web.DataAccess.IDBInterface
{
    public interface IGenericUnitofWorkRepository<T> where T : class
    {
        Task<Response<T>> GetByIdAsync(int id, string Quiery);
        Task<IReadOnlyList<T>> GetAllAsync(string Quiery);
        Task<Response<T>> AddAsync(T entity, string Quiery);
        Task<Response<T>> UpdateAsync(T entity, string Quiery);
        Task<Response<T>> DeleteAsync(int id, string Quiery);
        Task<List<T>> GetByNameAsync(string name, string Quiery);
    }
}
