using Newtonsoft.Json;
using NitoDeliveryService.ManagementPortal.Models.DTOs;
using NitoDeliveryService.ManagementPortal.Services.Infrastructure;
using NitoDeliveryService.ManagementPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NitoDeliveryService.ManagementPortal.Services.HttpClients
{
    public class Auth0ApiClient : IAuth0ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly Auth0PlaceManagementOptions _options;

        public Auth0ApiClient(Auth0PlaceManagementOptions options)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.Audience)
            };

            _options = options;
        }

        public async Task<Auth0CredentialsResponse> CreateUser(string email, int clientId, int slotId)
        {
            await EnsureAccessToken().ConfigureAwait(false);
            var password = GenerateRandomPassword();
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                email,
                password,
                connection = "LoginAuthentication",
                user_metadata = new
                {
                    clientId,
                    slotId
                }
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/v2/users", content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception($"Failed to create user. Status code: {response.StatusCode}. Error response: {errorResponse}");
            }

            return new Auth0CredentialsResponse()
            {
                auth0login = email,
                auth0password = password
            };
        }

        private async Task EnsureAccessToken()
        {
            var token = await GetClientCredentialsToken().ConfigureAwait(false);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task DeleteUser(string username)
        {
            await EnsureAccessToken().ConfigureAwait(false);

            var usersResponse = await _httpClient.GetAsync($"/api/v2/users?q=name:{Uri.EscapeDataString(username)}").ConfigureAwait(false);
            if (!usersResponse.IsSuccessStatusCode)
            {
                var errorResponse = await usersResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception($"Failed to retrieve user. Status code: {usersResponse.StatusCode}. Error response: {errorResponse}");
            }

            var users = await usersResponse.Content.ReadFromJsonAsync<IEnumerable<dynamic>>().ConfigureAwait(false);
            var user = users.FirstOrDefault();

            if (user is null)
            {
                throw new Exception($"User not found with username: {username}");
            }

            var userIdProperty = user.GetProperty("user_id");
            var userId = userIdProperty.GetString();

            if (userId is null)
            {
                throw new Exception($"User ID not found for username: {username}");
            }
            
            var response = await _httpClient.DeleteAsync($"/api/v2/users/{Uri.EscapeDataString(userId)}").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception($"Failed to delete user. Status code: {response.StatusCode}. Error response: {errorResponse}");
            }
        }

        private static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new Random();
            var password = new StringBuilder();
            for (int i = 0; i < 12; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }
            return password.ToString();
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
