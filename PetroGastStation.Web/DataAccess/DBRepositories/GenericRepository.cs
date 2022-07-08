using Microsoft.EntityFrameworkCore;
using PetroGastStation.Web.Data;
using PetroGastStation.Web.DataAccess.IDBInterface;
using System.Linq;
using System.Threading.Tasks;

namespace PetroGastStation.Web.DataAccess.DBRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _dataContext;

        public GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
            await SaveAllAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public async Task<IQueryable<T>> GetAll()
        {
            var urls = await _dataContext.Set<T>().ToListAsync();
            return urls.AsQueryable();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dataContext.Set<T>().Update(entity);
            await SaveAllAsync();
            return entity;
        }
    }
}
