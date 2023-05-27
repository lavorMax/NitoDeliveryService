using Newtonsoft.Json;
using NitoDeliveryService.PlaceManagementPortal.API.Infrastructure;
using NitoDeliveryService.PlaceManagementPortal.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class Auth0Client : IAuth0Client
    {
        private readonly HttpClient _httpClient;
        private readonly Auth0Options _options;
        private readonly ITokenParser _tokenParser;

        public Auth0Client(Auth0Options options, ITokenParser tokenParser)
        {
            _tokenParser = tokenParser;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.Audience)
            };

            _options = options;
        }

        public async Task<UserMetadata> GetMetadata()
        {
            await EnsureAccessToken().ConfigureAwait(false);

            var userId = _tokenParser.GetUserId();
            var apiUrl = $"https://{_options.Domain}api/v2/users/{userId}";

            var response = await _httpClient.GetAsync(apiUrl).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var jsonObject = JsonConvert.DeserializeObject<dynamic>(responseData);

                var clientId = (int)jsonObject.user_metadata.clientId;
                var placeId = (int)jsonObject.user_metadata.slotId;

                var userMetadata = new UserMetadata
                {
                    ClientId = clientId,
                    PlaceId = placeId
                };

                return userMetadata;
            }
            else
            {
                return null;
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
