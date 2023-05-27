using Newtonsoft.Json;
using NitoDeliveryService.ManagementPortal.Services.Infrastructure;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.HttpClients
{
    public class PlaceManagementPortalHttpClient : IPlaceManagementPortalHttpClient
    {
        private readonly PlaceManagementPortalOptions _options;
        private readonly Auth0PlaceManagementOptions _auth0Options;
        private readonly HttpClient _httpClient;

        public PlaceManagementPortalHttpClient(PlaceManagementPortalOptions options, Auth0PlaceManagementOptions auth0options)
        {
            _options = options;
            _auth0Options = auth0options;

            _httpClient = new HttpClient();
        }

        public async Task DeinitializeSlot(int clientId, int slotId)
        {
            await EnsureAccessToken().ConfigureAwait(false);

            var url = BuildUrl(_options.DeinitializeSlotEndpoint, clientId, slotId);

            var response = await _httpClient.DeleteAsync(url).ConfigureAwait(false);

            
            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalException($"Error occurred while calling the Denitialize endpoint. StatusCode={response.StatusCode}");
            }
        }

        public async Task InitializeSlot(int clientId, InitializeSlotRequest request)
        {
            await EnsureAccessToken().ConfigureAwait(false);

            var data = JsonConvert.SerializeObject(request);

            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var url = BuildUrl(_options.InitializeSlotEndpoint, clientId);

            var response = await _httpClient.PostAsync(url, content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalException($"Error occurred while calling the Denitialize endpoint. StatusCode={response.StatusCode}");
            }
        }

        private string BuildUrl(string endpoint, int clientId, int slotId = -1)
        {
            var builder = new StringBuilder(_options.PlaceManagementPortalURL);

            builder.Append(endpoint);
            builder.Append($"/{clientId}");

            if(slotId != -1)
            {
                builder.Append($"/{slotId}");
            }

            return builder.ToString();
        }

        private async Task EnsureAccessToken()
        {
            var token = await GetClientCredentialsToken().ConfigureAwait(false);
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

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            dynamic tokenResponse = JsonConvert.DeserializeObject(responseContent);

            return tokenResponse.access_token;
        }
    }
}
