using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetroGastStation.Common.Responses;
using PetroGastStation.Web.DataAccess.IDBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetroGastStation.Web.DataAccess.DBRepositories
{
    public class GenericUnitofWorkRepository<T> : IGenericUnitofWorkRepository<T> where T : class
    {
        private readonly IConfiguration _configuration;

        public GenericUnitofWorkRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response<T>> AddAsync(T entity, string Quiery)
        {
            // Set the time to the current moment


            // Basic SQL statement to insert a product into the products table
            //var sql = "Insert into Products (Name,Description,Barcode,Price,Added) VALUES (@Name,@Description,@Barcode,@Price,@Added)";

            // Sing the Dapper Connection string we open a connection to the database
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultPetroConnection")))
            {
                connection.Open();
                try
                {
                    // Pass the product object and the SQL statement into the Execute function (async)
                    var result = await connection.ExecuteAsync(Quiery, entity);
                    return new Response<T>
                    {
                        IsSuccess = result == 1 ? true : false,
                    };
                }
                catch (Exception exception)
                {
                    return new Response<T>
                    {
                        IsSuccess = false,
                        Message = exception.Message,
                    };
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<Response<T>> DeleteAsync(int id, string Quiery)
        {
            //var sql = "DELETE FROM Products WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultPetroConnection")))
            {
                try
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(Quiery, new { Id = id });
                    return new Response<T>
                    {
                        IsSuccess = result == 1 ? true : false,
                    };
                }
                catch (Exception exception)
                {
                    return new Response<T>
                    {
                        IsSuccess = false,
                        Message = exception.Message,
                    };
                }
                finally
                {
                    connection?.Close();
                }
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(string Quiery)
        {
            //var sql = "SELECT * FROM Products";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultPetroConnection")))
            {
                try
                {
                    connection.Open();

                    // Map all products from database to a list of type Product defined in Models.
                    // this is done by using Async method which is also used on the GetByIdAsync
                    var result = await connection.QueryAsync<T>(Quiery);
                    return result.ToList();
                }
                catch (Exception exception)
                {

                    throw;
                }
                finally { connection.Close(); }
            }
        }

        public async Task<Response<T>> GetByIdAsync(int id, string Quiery)
        {
            //var sql = "SELECT * FROM Products WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultPetroConnection")))
            {
                try
                {
                    connection.Open();
                    var result = await connection.QuerySingleOrDefaultAsync<T>(Quiery, new { Id = id });
                    return new Response<T>
                    {
                        IsSuccess = result == null ? false : true,
                        //Result = (T)result,
                    };
                }
                catch (Exception exception)
                {
                    return new Response<T>
                    {
                        IsSuccess = false,
                        Message = exception.Message,
                    };
                }
                finally
                {
                }
            }
        }

        public async Task<List<T>> GetByNameAsync(string name, string Quiery)
        {
            //var sql = "SELECT * FROM [dbo].[Products] WHERE EspecialidadNombre = @EspecialidadNombre";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultPetroConnection")))
            {
                try
                {
                    connection.Open();
                    var result = await connection.QueryAsync<T>(Quiery, new { EspecialidadNombre = name });
                    return result.ToList();
                }
                catch (System.Exception)
                {
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<Response<T>> UpdateAsync(T entity, string Quiery)
        {
            //var sql = "UPDATE Products SET Name = @Name, Description = @Description, Barcode = @Barcode, Price = @Price, Modified = @Modified  WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultPetroConnection")))
            {
                try
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(Quiery, entity);
                    return new Response<T>
                    {
                        IsSuccess = result == 1 ? true : false,
                    };
                }
                catch (Exception exception)
                {
                    return new Response<T>
                    {
                        IsSuccess = false,
                        Message = exception.Message,
                    };
                }
                finally
                {
                    connection?.Close();
                }
            }
        }
    }
}
