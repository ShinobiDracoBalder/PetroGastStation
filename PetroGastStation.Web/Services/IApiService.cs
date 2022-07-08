using PetroGastStation.Common.Responses;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Services
{
    public interface IApiService
    {
        Task<Response<T>> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
        //Task<bool> CheckConnectionAsync(string url);
    }
}
