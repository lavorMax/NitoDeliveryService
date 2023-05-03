using Newtonsoft.Json;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.HttpClients
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

        public async Task<PlaceDTO> Get(int placeId, int clientId)
        {
            var url = BuildUrl(_options.GetPlaceEndpoint);

            url += $"?placeId={placeId}&clientId={clientId}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<PlaceDTO>(responseContent);
            }
            else
            {
                throw new Exception($"Error occurred while calling the Get endpoint. StatusCode={response.StatusCode}");
            }
        }

        private string BuildUrl(string endpoint)
        {
            var builder = new StringBuilder(_options.PlaceManagementPortalURL);

            builder.Append(endpoint);

            return builder.ToString();
        }
    }
}
