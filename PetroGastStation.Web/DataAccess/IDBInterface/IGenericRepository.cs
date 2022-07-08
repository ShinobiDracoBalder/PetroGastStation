using System.Linq;
using System.Threading.Tasks;

namespace PetroGastStation.Web.DataAccess.IDBInterface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> SaveAllAsync();
    }
}
