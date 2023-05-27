using Newtonsoft.Json;
using NitoDeliveryService.DeliveryServicePortal.API.Infrastructure;
using NitoDeliveryService.DeliveryServicePortal.Services.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.DeliveryServicePortal.Services.HttpClients
{
    public class Auth0Client : IAuth0Client
    {
        private readonly HttpClient _httpClient;
        private readonly Auth0Options _options;

        public Auth0Client(Auth0Options options)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.Audience)
            };

            _options = options;
        }

        public async Task CreateUser(UserDTO user)
        {
            await EnsureAccessToken().ConfigureAwait(false);
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                email = user.Email,
                password = user.Password,
                connection = "DeliveryServiceConection",
                user_metadata = new
                {
                    user.Id
                }
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/v2/users", content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception($"Failed to create user. Status code: {response.StatusCode}. Error response: {errorResponse}");
            }
        }

        private async Task EnsureAccessToken()
        {
            var token = await GetClientCredentialsToken().ConfigureAwait(false);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<string> GetClientCredentialsToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_options.Domain}oauth/token");

            var body = new
            {
                grant_type = "client_credentials",
                client_id = _options.ClientId,
                client_secret = _options.ClientSecret,
                audience = _options.Audience
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            dynamic tokenResponse = JsonConvert.DeserializeObject(responseContent);

            return tokenResponse.access_token;
        }
    }
}
