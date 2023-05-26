using Newtonsoft.Json;
using NitoDeliveryService.PlaceManagementPortal.Services.Infrasctructure;
using NitoDeliveryService.PlaceManagementPortal.Services.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.Models;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.HttpClients
{
    public class DeliveryServiceHttpClient : IDeliveryServiceHttpClient
    {
        private readonly DeliveryServiceOptions _options;
        private readonly Auth0DeliveryServiceOptions _auth0Options;
        private readonly HttpClient _httpClient;

        public DeliveryServiceHttpClient(DeliveryServiceOptions options, Auth0DeliveryServiceOptions auth0options)
        {
            _options = options;
            _auth0Options = auth0options;

            _httpClient = new HttpClient();
        }

        public async Task ChangeStatus(int orderId, OrderStatuses status)
        {
            await EnsureAccessToken();

            var data = JsonConvert.SerializeObject(status);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = BuildUrl(_options.ChangeOrderStatusEndpoint);

            url += $"?orderId={orderId}";

            var response = await _httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error occurred while calling the CreatePlace endpoint. StatusCode={response.StatusCode}");
            }
        }

        public async Task CreatePlace(PlaceDTO place)
        {
            await EnsureAccessToken();

            var data = JsonConvert.SerializeObject(place);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = BuildUrl(_options.CreatePlaceEndpoint);

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error occurred while calling the CreatePlace endpoint. StatusCode={response.StatusCode}");
            }
        }

        public async Task DeletePlace(int placeId, int clientId)
        {
            await EnsureAccessToken();

            var url = BuildUrl(_options.DeletePlaceEndpoint)+$"/{placeId}/{clientId}";

            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error occurred while calling the DeletePlace endpoint. StatusCode={response.StatusCode}");
            }
        }

        public async Task<OrderDTO> GetOrder(int orderId)
        {
            await EnsureAccessToken();

            var url = BuildUrl(_options.GetOrderEndpoint);

            url += $"?id={orderId}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<OrderDTO>(responseContent);
            }
            else
            {
                throw new Exception($"Error occurred while calling the GetOrder endpoint. StatusCode={response.StatusCode}");
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrders(int placeId, int clientId, bool onlyActive = true)
        {
            await EnsureAccessToken();

            var url = BuildUrl(_options.GetOrdersEndpoint);

            url += $"?clientId={clientId}&placeId={placeId}&onlyActive={onlyActive}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseContent);
            }
            else
            {
                throw new Exception($"Error occurred while calling the GetOrders endpoint. StatusCode={response.StatusCode}");
            }
        }

        public async Task UpdatePlace(PlaceDTO place)
        {
            await EnsureAccessToken();

            var data = JsonConvert.SerializeObject(place);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = BuildUrl(_options.UpdatePlaceEndpoint);

            var response = await _httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error occurred while calling the CreatePlace endpoint. StatusCode={response.StatusCode}");
            }
        }

        private string BuildUrl(string endpoint)
        {
            var builder = new StringBuilder(_options.DeliveryServiceURL);

            builder.Append(endpoint);

            return builder.ToString();
        }

        private async Task EnsureAccessToken()
        {
            var token = await GetClientCredentialsToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<string> GetClientCredentialsToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_auth0Options.Domain}oauth/token");

            var body = new
            {
                grant_type = "client_credentials",
                client_id = _auth0Options.ClientId,
                client_secret = _auth0Options.ClientSecret,
                audience = _auth0Options.Audience
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = JsonConvert.DeserializeObject(responseContent);

            return tokenResponse.access_token;
        }
    }
}
