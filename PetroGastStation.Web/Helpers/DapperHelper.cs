using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetroGastStation.Common.Responses;
using PetroGastStation.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Helpers
{
    public class DapperHelper : IDapperHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IListERP _listERP;
        private string Connectionstring = "DefaultPetroConnection";
        public DapperHelper(IConfiguration configuration, IListERP listERP)
        {
            _configuration = configuration;
            _listERP = listERP;
        }
        public void Dispose()
        { }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new System.NotImplementedException();
        }
        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public async Task<GenericResponse<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {

            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var list = await  db.QueryAsync<T>(sp, parms, commandType: commandType);
                return new GenericResponse<T>
                {
                    IsSuccess = true,
                    ListResults = list.ToList(),
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
        }
        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_configuration.GetConnectionString(Connectionstring));
        }

        public Response<T> GetOnly<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                result = db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Response<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return new Response<T>
            {
                IsSuccess = true,
                Result = result,
            };
        }

        public Response<T> GetOnlyAvatar<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            T result;
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                result = db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Response<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return new Response<T>
            {
                IsSuccess = true,
                Result = result,
            };
        }

        public async Task<Response<T>> GetPlacesModifTransactionAsync<T>(List<StationPipeViewModel> ModifgasSt)
        {
            string sql = string.Empty;
            int affectedRows = 0;
            int affectedRow = 0;
            string _GasStation = string.Empty;
            using (var connection = new SqlConnection(_configuration.GetConnectionString(Connectionstring)))
            {
                try
                {
                    connection.Open();
                    sql = $"{"Update [dbo].[TblPipeReports] Set TAR=@TAR, Remision=@Remision,Factura=@Factura,Fecha=@Fecha,	Vencimiento=@Vencimiento, Combustible=@Combustible,	LtsRecibidos=@LtsRecibidos,	Litros=@Litros,	Subtotal=@Subtotal,	IVA=@IVA,	IEPS=@IEPS,	Flete=@Flete,IVAFlete=@IVAFlete,Ret=@Ret,Descuento=@Descuento,Total=@Total,RegisterDate=@RegisterDate  "}{" Where PipeReportId=@PipeReportId and NumeroPermiso =@NumeroPermiso;"}";
                    using (var transaction = connection.BeginTransaction())
                    {
                        var _GasSta = _listERP.GetAllAsync();



                        foreach (var item in ModifgasSt.ToList().OrderBy(g => g.Rows))
                        {
                            _GasStation = item.GasStation;
                            var _FirstGas = _GasSta.FirstOrDefault(g => g.ERP.Contains(item.GasStation));
                            affectedRow = await connection.ExecuteAsync(sql, new {  TAR = item.TAR, Remision = item.Remision, Factura = item.Factura, Fecha = item.Fecha, Vencimiento = item.Vencimiento, Combustible = item.Combustible, LtsRecibidos = item.LtsRecibidos, Litros = item.Litros, Subtotal = item.Subtotal, IVA = item.IVA, IEPS = item.IEPS, Flete = item.Flete, IVAFlete = item.IVAFlete, Ret = item.Ret, Descuento = item.Descuento, Total = item.Total, RegisterDate = item.RegisterDate, PipeReportId =item.PipeReportId, NumeroPermiso = item.NumeroPermiso }, transaction: transaction);
                            affectedRows = affectedRows + affectedRow;
                            Console.WriteLine(affectedRows);
                        }

                        transaction.Commit();

                        return new Response<T>
                        {
                            IsSuccess = true,
                        };
                    }
                }
                catch (Exception exception)
                {

                    return new Response<T>
                    {
                        IsSuccess = false,
                        Message = $"Error : {exception.Message}  Error ERP:{_GasStation} "
                    };
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<Response<T>> GetPlacesTransactionAsync<T>(List<StationPipeViewModel> gasSt)
        {
            string sql = string.Empty;
            int affectedRows = 0;
            int affectedRow = 0;
            string _GasStation = string.Empty;
            using (var connection = new SqlConnection(_configuration.GetConnectionString(Connectionstring)))
            {
                try
                {
                    connection.Open();
                    sql = $"{"Insert Into [dbo].[TblPipeReports] (RazonSocial,	NumeroPermiso,	Apelativo,TAR,Remision,Factura,Fecha,Vencimiento,Combustible,LtsRecibidos,Litros,Subtotal,IVA,IEPS,Flete,IVAFlete,	Ret,Descuento,Total,Rows,Deleted,RegisterDate) "}{" Values(@RazonSocial,@NumeroPermiso,	@Apelativo,  @TAR, @Remision,@Factura,@Fecha,	@Vencimiento,	@Combustible,	@LtsRecibidos,	@Litros,	@Subtotal,	@IVA,	@IEPS,	@Flete,	@IVAFlete,	@Ret,	@Descuento,	@Total,@Rows,@Deleted,@RegisterDate);"}";
                    using (var transaction = connection.BeginTransaction())
                    {
                        var _GasSta = _listERP.GetAllAsync();

                       

                        foreach (var item in gasSt.ToList().OrderBy(g => g.Rows))
                        {
                            _GasStation = item.GasStation;
                            var _FirstGas = _GasSta.FirstOrDefault(g => g.ERP.Contains(item.GasStation));
                            affectedRow = await connection.ExecuteAsync(sql, new { RazonSocial = _FirstGas.RazonSocial, Apelativo = item.GasStation, NumeroPermiso = _FirstGas.NumeroPermiso, GasStation = item.GasStation,TAR = item.TAR, Remision = item.Remision, Factura = item.Factura, Fecha = item.Fecha, Vencimiento = item.Vencimiento, Combustible = item.Combustible, LtsRecibidos = item.LtsRecibidos, Litros = item.Litros,  Subtotal = item.Subtotal, IVA = item.IVA,   IEPS = item.IEPS, Flete = item.Flete,  IVAFlete = item.IVAFlete, Ret = item.Ret, Descuento = item.Descuento, item.Total, item.Rows,Deleted = item.Deleted, RegisterDate = item.RegisterDate }, transaction: transaction);
                            affectedRows = affectedRows + affectedRow;
                            Console.WriteLine(affectedRows);
                        }

                        transaction.Commit();

                        return new Response<T>
                        {
                            IsSuccess = true,
                        };
                    }
                }
                catch (Exception exception)
                {

                    return new Response<T> {
                        IsSuccess = false,
                        Message = $"Error : {exception.Message}  Error ERP:{_GasStation} "
                    };
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Response<T> Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new Response<T>
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return new Response<T>
            {
                IsSuccess = true,
                Result = result,
            };
        }

        public Task<Response<T>> PostListAddGasAsync<T>(List<T> entity, string Quiery)
        {

            throw new NotImplementedException();
        }

        public Response<T> Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return new Response<T>
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return new Response<T>
            {
                IsSuccess = true,
                Result = result,
            };
        }
    }
}
