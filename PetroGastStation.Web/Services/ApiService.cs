using Newtonsoft.Json;
using PetroGastStation.Common.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Services
{
    public class ApiService : IApiService
    {
        //public async Task<bool> CheckConnectionAsync(string url)
        //{
        //    if (!CrossConnectivity.Current.IsConnected)
        //    {
        //        return false;
        //    }

        //    return await CrossConnectivity.Current.IsRemoteReachable(url);
        //}

        public async Task<Response<T>> GetListAsync<T>(string urlBase, string servicePrefix, string controller)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<T>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                List<T> list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response<T>{
                    IsSuccess = true,
                    ResultList = list,
                };
            }
            catch (Exception ex)
            {
                return new Response<T>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
