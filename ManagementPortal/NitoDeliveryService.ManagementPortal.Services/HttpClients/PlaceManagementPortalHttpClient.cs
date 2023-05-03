using Newtonsoft.Json;
using NitoDeliveryService.ManagementPortal.Services.Infrastructure;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.HttpClients
{
    public class PlaceManagementPortalHttpClient : IPlaceManagementPortalHttpClient
    {
        private readonly PlaceManagementPortalOptions _options;
        private readonly HttpClient _httpClient;

        public PlaceManagementPortalHttpClient(PlaceManagementPortalOptions options, Auth0PlaceManagementOptions auth0options)
        {
            _options = options;

            _httpClient = new HttpClient();

            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{auth0options.ClientId}:{auth0options.ClientSecret}")));
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;
        }

        public async Task DeinitializeSlot(int clientId, int slotId)
        {

            var url = BuildUrl(_options.DeinitializeSlotEndpoint, clientId, slotId);

            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error occurred while calling the Denitialize endpoint. StatusCode={response.StatusCode}");
            }
        }

        public async Task InitializeSlot(int clientId, InitializeSlotRequest request)
        {
            var data = JsonConvert.SerializeObject(request);

            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var url = BuildUrl(_options.InitializeSlotEndpoint, clientId);

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error occurred while calling the Denitialize endpoint. StatusCode={response.StatusCode}");
            }
        }

        private string BuildUrl(string endpoint, int clientId, int slotId = -1)
        {
            var builder = new StringBuilder(_options.PlaceManagementPortalURL);

            builder.Append(endpoint);
            builder.Append($"?clientId={clientId}");

            if(slotId != -1)
            {
                builder.Append($"&slotId={slotId}");
            }

            return builder.ToString();
        }
    }
}
