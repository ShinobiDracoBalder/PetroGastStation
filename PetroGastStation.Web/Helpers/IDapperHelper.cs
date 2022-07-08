using Dapper;
using PetroGastStation.Common.Responses;
using PetroGastStation.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Helpers
{
    public interface IDapperHelper : IDisposable
    {
        DbConnection GetDbconnection();
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Response<T> Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Response<T> Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<GenericResponse<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        Response<T> GetOnly<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Response<T> GetOnlyAvatar<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);

        Task<Response<T>> PostListAddGasAsync<T>(List<T> entity, string Quiery);
        Task<Response<T>> GetPlacesTransactionAsync<T>(List<StationPipeViewModel> gasSt);
        Task<Response<T>> GetPlacesModifTransactionAsync<T>(List<StationPipeViewModel> ModifgasSt);
    }
}
