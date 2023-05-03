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
        private readonly HttpClient _httpClient;

        public DeliveryServiceHttpClient(DeliveryServiceOptions options, Auth0DeliveryServiceOptions auth0options)
        {
            _options = options;

            _httpClient = new HttpClient();

            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{auth0options.ClientId}:{auth0options.ClientSecret}")));
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;
        }

        public async Task ChangeStatus(int orderId, OrderStatuses status)
        {
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
            var data = JsonConvert.SerializeObject(place);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var url = BuildUrl(_options.CreatePlaceEndpoint);

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error occurred while calling the CreatePlace endpoint. StatusCode={response.StatusCode}");
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrders(int placeId, int clientId, bool onlyActive = true)
        {
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
    }
}
