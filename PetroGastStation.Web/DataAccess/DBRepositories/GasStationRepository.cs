using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetroGastStation.Common.Responses;
using PetroGastStation.Web.DataAccess.IDBInterface;
using PetroGastStation.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PetroGastStation.Web.DataAccess.DBRepositories
{
    public class GasStationRepository : GenericUnitofWorkRepository<GasStationViewModel>, IGasStationRepository
    {
        private readonly IConfiguration _configuration;

        public GasStationRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public Task<Response<GasStationViewModel>> AllGasStationByAsync(string SqlQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GasStationViewModel>> GetGasStationByNameAsync(int Id, string SqlQuery)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultPetroConnection")))
            {
                try
                {
                    connection.Open();
                    List<GasStationViewModel> castResults = new List<GasStationViewModel>();
                    var param = new DynamicParameters();
                    param.Add("@Id", dbType: DbType.Int32, value: Id, direction: ParameterDirection.Input);
                    castResults = (List<GasStationViewModel>)await connection.QueryAsync<GasStationViewModel>(SqlQuery, param, commandType: CommandType.StoredProcedure);
                    return castResults.ToList();
                }
                catch (System.Exception)
                {

                    throw;
                }
                finally { connection.Close(); }
            }
        }

        public async Task<Response<GasStationViewModel>> GetGasStationByOnlyAsync(int Id, string SqlQuery)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultPetroConnection")))
            {
                try
                {
                    connection.Open();
                    DynamicParameters parameter = new DynamicParameters();


                    parameter.Add("@DistributorId", Id, DbType.Int32, ParameterDirection.Input);
                    //parameter.Add("@Code", "Many_Insert_0", DbType.String, ParameterDirection.Input);
                    // parameter.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                    ///var orderDetail = await connection.QueryFirstOrDefaultAsync<DistributorViewModel>(SqlQuery, parameter,commandType: CommandType.Text);
                    var orderDetail = await connection.QueryAsync<GasStationViewModel>(SqlQuery, parameter, commandType: CommandType.Text);
                    if (orderDetail == null)
                    {
                        return new Response<GasStationViewModel>
                        {
                            IsSuccess = false,
                            Message = "No Data"
                        };
                    }
                    return new Response<GasStationViewModel>
                    {
                        IsSuccess = false,
                        ResultList = orderDetail.ToList(),
                    };
                }
                catch (System.Exception exception)
                {
                    return new Response<GasStationViewModel>
                    {
                        IsSuccess = false,
                        Message = exception.Message
                    };
                }
                finally { connection.Close(); }
            }
        }

        public Task<IEnumerable<GasStationViewModel>> GetGasStationByOrderNameAsync(string orderName, string SqlQuery)
        {
            throw new NotImplementedException();
        }
    }
}
