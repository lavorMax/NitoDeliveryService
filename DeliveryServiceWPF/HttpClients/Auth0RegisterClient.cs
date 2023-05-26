using Newtonsoft.Json;
using NitoDeliveryService.Shared.HttpClients;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceWPF.HttpClients
{
    public class Auth0RegisterClient : Auth0Client, IAuth0RegisterClient
    {
        public Auth0RegisterClient(Auth0Options options) : base(options)
        {
        }
        public async Task<string> CreateUser(string email, string password, int userId)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                email,
                password,
                connection = "Username-Password-Authentication",
                app_metadata = new
                {
                    userId
                }
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/v2/users", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create user. Status code: {response.StatusCode}. Error response: {errorResponse}");
            }

            return await Authenticate(email, password);
        }
    }
}
