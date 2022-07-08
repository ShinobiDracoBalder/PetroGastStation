using Microsoft.Extensions.DependencyInjection;
using PetroGastStation.Web.DataAccess.IDBInterface;

namespace PetroGastStation.Web.DataAccess.DBRepositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<IGasStationRepository, GasStationRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
