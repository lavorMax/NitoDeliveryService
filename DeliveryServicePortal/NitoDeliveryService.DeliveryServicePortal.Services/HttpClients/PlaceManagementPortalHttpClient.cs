using Newtonsoft.Json;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.HttpClients
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

        public async Task<PlaceDTO> Get(int placeId, int clientId)
        {
            await EnsureAccessToken().ConfigureAwait(false);
            
            var url = BuildUrl(_options.GetPlaceEndpoint);

            url += $"/{placeId}/{clientId}";

            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<PlaceDTO>(responseContent);
            }
            else
            {
                throw new ExternalException($"Error occurred while calling the Get endpoint. StatusCode={response.StatusCode}");
            }
        }

        private string BuildUrl(string endpoint)
        {
            var builder = new StringBuilder(_options.PlaceManagementPortalURL);

            builder.Append(endpoint);

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
